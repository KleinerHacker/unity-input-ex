using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Assets;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Types;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils.Extensions;
using InputAction = UnityInputEx.Runtime.input_ex.Scripts.Runtime.Types.InputAction;
using InputValue = UnityInputEx.Runtime.input_ex.Scripts.Runtime.Assets.InputValue;

// ReSharper disable HeapView.BoxingAllocation

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components
{
    [AddComponentMenu(UnityInputConstants.Root + "/Asset Input System")]
    [DisallowMultipleComponent]
    public sealed class AssetInputSystem : MonoBehaviour
    {
        #region Inpsector Data

        [SerializeField]
        private InputPreset preset;

        #endregion

        private readonly IList<RuntimeControl> _runtimeControlList = new List<RuntimeControl>();

        #region Builtin Methods

        private void Awake()
        {
            foreach (var item in preset.Items)
            {
                HandleItem(item);
            }
        }

        private void Update()
        {
            foreach (var runtimeControl in _runtimeControlList)
            {
                runtimeControl.Run();
            }
        }

        #endregion
 
        private void HandleItem(InputItem item)
        {
            var runtimeControl = CreateControlFromItem(item);
            if (runtimeControl == null)
                return;
            
            _runtimeControlList.Add(runtimeControl);
        }

        private RuntimeControl CreateControlFromItem(InputItem item)
        {
            InputDevice inputDevice = item.Type switch
            {
                InputType.Mouse => Mouse.current,
                InputType.Keyboard => Keyboard.current,
                InputType.Pointer => Pointer.current,
                InputType.Touchscreen => Touchscreen.current,
                InputType.Gamepad => Gamepad.current,
                _ => throw new NotImplementedException()
            };
            if (inputDevice.deviceId == InputDevice.InvalidDeviceId)
                return null;

            var inputElementPI = inputDevice.GetType().GetProperty(item.Field);
            if (inputElementPI == null)
                throw new InvalidOperationException("Unable to find property " + item.Field + " in " + item.Type);
            var inputElement = (InputControl) inputElementPI.GetValue(inputDevice);


            (object defValue, TryGetValueDelegate getter) value = item.Value switch
            {
                InputValue.Button =>
                    item.Behavior switch
                    {
                        InputBehavior.Press => (false, (ref object oldValue, out object value) =>
                        {
                            var success = ((ButtonControl) inputElement).wasPressedThisFrame;
                            value = success;
                            //Debug.Log("*** " + success + " / " + value);

                            return success;
                        }),
                        InputBehavior.Release => (false, (ref object oldValue, out object value) =>
                        {
                            var success = ((ButtonControl) inputElement).wasReleasedThisFrame;
                            value = success;

                            return success;
                        }),
                        InputBehavior.Hold => (false, (ref object oldValue, out object value) =>
                        {
                            var success = ((ButtonControl) inputElement).isPressed;
                            value = success;

                            return success;
                        }),
                        _ => throw new NotImplementedException(),
                    },
                InputValue.Float => (0f, (ref object oldValue, out object value) =>
                {
                    value = ((AxisControl) inputElement).ReadValue();
                    var success = value.GetHashCode() != oldValue.GetHashCode();
                    oldValue = value;

                    return success;
                }),
                InputValue.Integer => (0, (ref object oldValue, out object value) =>
                {
                    value = ((IntegerControl) inputElement).ReadValue();
                    var success = value.GetHashCode() != oldValue.GetHashCode();
                    oldValue = value;

                    return success;
                }),
                InputValue.Vector2 => (Vector2.zero, (ref object oldValue, out object value) =>
                {
                    value = ((Vector2Control) inputElement).ReadValue();
                    var success = value.GetHashCode() != oldValue.GetHashCode();
                    oldValue = value;

                    return success;
                }),
                _ => throw new NotImplementedException(),
            };

            var inputActions = item.Actions.Select(x => x.ToInputAction()).ToArray();
            var subControls = item.SubItems.Select(CreateControlFromItem).ToArray();
            
            return new RuntimeControl(item.Name, value.getter, value.defValue, inputActions, subControls);
        }

        private sealed class RuntimeControl
        {
            private readonly string _name;
            private readonly TryGetValueDelegate _getter;
            private readonly InputAction[] _inputActions;
            private readonly RuntimeControl[] _subControls;

            private object _oldValue;

            public RuntimeControl(string name, TryGetValueDelegate getter, object oldValue, InputAction[] inputActions, RuntimeControl[] subControls)
            {
                _name = name;
                _getter = getter;
                _oldValue = oldValue;
                _inputActions = inputActions;
                _subControls = subControls;
            }

            public bool Run()
            {
                return Run(new Dictionary<string, object>());
            }

            private bool Run(IDictionary<string, object> values)
            {
                var success = _getter(ref _oldValue, out var value);
                if (success)
                {
                    values.Add(_name, value);
                    
                    if (_subControls.Any(x => x.Run(values)))
                        return true; //Break recursion if sub control hits (priority for sub controls)
                    
                    foreach (var inputAction in _inputActions)
                    {
                        inputAction.RaisePerform(new InputActionContext(inputAction, values));
                    }

                    return true;
                }

                return false;
            }
        }

        private delegate bool TryGetValueDelegate(ref object oldValue, out object value);
    }
}
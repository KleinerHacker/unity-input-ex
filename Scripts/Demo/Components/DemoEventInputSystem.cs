using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils.Extensions;

namespace UnityInputEx.Demo.input_ex.Scripts.Demo.Components
{
    public sealed class DemoEventInputSystem : EventInputSystem<DemoEventInputSystem.IDemoInput, DemoEventInputSystem>
    {
        protected override IDemoInput[] Inputs { get; } = new IDemoInput[] { new KeyboardInput(), new GamepadInput() };

        [EventInputMember(nameof(Identifier.Pressed))]
        public event EventHandler OnPressed;

        [EventInputMember(nameof(Identifier.Activated), Type = EventInputMemberType.Activation)]
        public event EventHandler<ActivationInputEventArgs> OnActivated;

        [EventInputMember(nameof(Identifier.Axis), Type = EventInputMemberType.Axis)]
        public event EventHandler<AxisInputEventArgs> OnAxis;
        
        [EventInputMember(nameof(Identifier.Axis2D), Type = EventInputMemberType.Axis2D)]
        public event EventHandler<Axis2DInputEventArgs> OnAxis2D;
        
        public interface IDemoInput : IEventInput
        {
            [EventInputMember(nameof(Identifier.Pressed))]
            bool IsPressed();

            [EventInputMember(nameof(Identifier.Activated), Type = EventInputMemberType.Activation)]
            bool IsActivated();

            [EventInputMember(nameof(Identifier.Axis), Type = EventInputMemberType.Axis)]
            float GetAxis();
            
            [EventInputMember(nameof(Identifier.Axis2D), Type = EventInputMemberType.Axis2D)]
            Vector2 GetAxis2D();
        }

        private sealed class KeyboardInput : IDemoInput
        {
            public bool IsAvailable => Keyboard.current.IsAvailable();

            public bool IsPressed()
            {
                return Keyboard.current.xKey.wasPressedThisFrame;
            }

            public bool IsActivated()
            {
                return Keyboard.current.pKey.isPressed;
            }

            public float GetAxis()
            {
                return Keyboard.current.numpad4Key.isPressed ? -1f : Keyboard.current.numpad6Key.isPressed ? 1f : 0f;
            }

            public Vector2 GetAxis2D()
            {
                return new Vector2(
                    Keyboard.current.leftArrowKey.isPressed ? -1f : Keyboard.current.rightArrowKey.isPressed ? 1f : 0f,
                    Keyboard.current.upArrowKey.isPressed ? -1f : Keyboard.current.downArrowKey.isPressed ? 1f : 0f
                );
            }
        }

        private sealed class GamepadInput : IDemoInput
        {
            public bool IsAvailable => Gamepad.current.IsAvailable();
            
            public bool IsPressed()
            {
                return Gamepad.current.aButton.wasPressedThisFrame;
            }

            public bool IsActivated()
            {
                return Gamepad.current.xButton.isPressed;
            }

            public float GetAxis()
            {
                return Gamepad.current.leftTrigger.ReadValue();
            }

            public Vector2 GetAxis2D()
            {
                return Gamepad.current.leftStick.ReadValue();
            }
        }

        private enum Identifier
        {
            Pressed,
            Activated,
            Axis,
            Axis2D,
        }
    }
}
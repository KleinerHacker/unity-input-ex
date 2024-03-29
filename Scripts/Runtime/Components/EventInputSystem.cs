using System;
using System.Collections.Generic;
using System.Linq;
using UnityBase.Runtime.@base.Scripts.Runtime.Components.Singleton;
using UnityBase.Runtime.@base.Scripts.Runtime.Components.Singleton.Attributes;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components
{
    public interface IEventInputSystem
    {
    }
    
    [Singleton(Scope = SingletonScope.Application, Instance = SingletonInstance.RequiresNewInstance, CreationTime = SingletonCreationTime.Loading, ObjectName = "Event Input System")]
    public abstract partial class EventInputSystem<T,TS> : SingletonBehavior<TS>, IEventInputSystem where T : class, EventInputSystem<T, TS>.IEventInput where TS : EventInputSystem<T,TS>
    {
        protected abstract T[] Inputs { get; }

        private T[] _availableInputs;
        private IList<InputReflectionData> _data;

        #region Builtin Methods

        protected override void DoAwake()
        {
            _availableInputs = Inputs.Where(x => x.IsAvailable).ToArray();
            _data = ReadReflectionData();
        }

        private void LateUpdate()
        {
            foreach (var value in _data)
            {
                switch (value.Type)
                {
                    case EventInputMemberType.Simple:
                        HandleSimple(value);
                        break;
                    case EventInputMemberType.Activation:
                        HandleActivation(value);
                        break;
                    case EventInputMemberType.Axis:
                        HandleAxis(value);
                        break;
                    case EventInputMemberType.Axis2D:
                        HandleAxis2D(value);
                        break;
                    case EventInputMemberType.Point:
                        HandlePoint(value);
                        break;
                    default:
                        throw new NotImplementedException(value.Type.ToString());
                }
            }
        }

        #endregion

        private void HandleSimple(InputReflectionData data)
        {
            var success = (
                    from input in _availableInputs
                    from method in data.CheckMethods
                    select (bool)method.Invoke(input, Array.Empty<object>())
                )
                .Any(result => result);
            
            if (success)
            {
                foreach (var @event in data.RiseEvents)
                {
                    @event.Raise(this, EventArgs.Empty);
                }
            }
        }

        private void HandleActivation(InputReflectionData data)
        {
            var success = (
                    from input in _availableInputs
                    from method in data.CheckMethods
                    select (bool)method.Invoke(input, Array.Empty<object>())
                )
                .Any(result => result);
            
            if (success)
            {
                foreach (var @event in data.RiseEvents)
                {
                    @event.Raise(this, new ActivationInputEventArgs(ActivationInputEventArgs.ActivationType.Activated));
                }
            }
            else
            {
                foreach (var @event in data.RiseEvents)
                {
                    @event.Raise(this, new ActivationInputEventArgs(ActivationInputEventArgs.ActivationType.Deactivated));
                }
            }
        }

        private void HandleAxis(InputReflectionData data)
        {
            var value = (
                from input in _availableInputs
                from method in data.CheckMethods
                select (float)method.Invoke(input, Array.Empty<object>())
            ).Sum();

            foreach (var @event in data.RiseEvents)
            {
                @event.Raise(this, new AxisInputEventArgs(value));
            }
        }

        private void HandleAxis2D(InputReflectionData data)
        {
            var value = (
                    from input in _availableInputs
                    from method in data.CheckMethods
                    select (Vector2)method.Invoke(input, Array.Empty<object>())
                )
                .Aggregate((x, y) => x + y);

            foreach (var @event in data.RiseEvents)
            {
                @event.Raise(this, new Axis2DInputEventArgs(value));
            }
        }
        
        private void HandlePoint(InputReflectionData data)
        {
            var value = (
                    from input in _availableInputs
                    from method in data.CheckMethods
                    select (Vector2?)method.Invoke(input, Array.Empty<object>())
                )
                .FirstOrDefault(x => x != null);
            if (value == null)
                return;

            foreach (var @event in data.RiseEvents)
            {
                @event.Raise(this, new Axis2DInputEventArgs(value.Value));
            }
        }
    }
}
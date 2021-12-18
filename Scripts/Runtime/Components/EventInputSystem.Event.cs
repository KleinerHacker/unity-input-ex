using System;
using UnityEngine;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components
{
    public abstract partial class EventInputSystem<T,TS>
    {
        public interface IEventInput
        {
            bool IsAvailable { get; }
        }
    }
    
    /// <summary>
    /// Use this for events marked with <see cref="EventInputMemberType.Activation"/>
    /// </summary>
    public class ActivationInputEventArgs : EventArgs
    {
        public ActivationType Type { get; }

        public ActivationInputEventArgs(ActivationType type)
        {
            Type = type;
        }

        public enum ActivationType
        {
            Activated,
            Deactivated
        }
    }

    /// <summary>
    /// Use this for events marked with <see cref="EventInputMemberType.Axis"/>
    /// </summary>
    public class AxisInputEventArgs : EventArgs
    {
        public float Value { get; }

        public AxisInputEventArgs(float value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Use this for events marked with <see cref="EventInputMemberType.Axis2D"/> or <see cref="EventInputMemberType.Point"/>
    /// </summary>
    public class Axis2DInputEventArgs : EventArgs
    {
        public Vector2 Value { get; }

        public Axis2DInputEventArgs(Vector2 value)
        {
            Value = value;
        }
    }
}
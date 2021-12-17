using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components
{
    public abstract partial class EventInputSystem<T,TS>
    {
        private void ReadReflectionData()
        {
            ReadMethodReflectionData();
            ReadEventReflectionData();
        }

        private void ReadEventReflectionData()
        {
            foreach (var eventInfo in GetType().GetEvents())
            {
                var attribute = eventInfo.GetCustomAttribute<EventInputMemberAttribute>();
                if (attribute == null)
                    continue;

                if (!_data.ContainsKey(attribute.Identifier))
                {
                    _data.Add(attribute.Identifier, new InputReflectionData(attribute.Identifier, attribute.Type));
                }
                
                ValidateEvent(eventInfo, attribute.Type);
                ValidateData(attribute.Identifier, attribute.Type);
                
                _data[attribute.Identifier].RiseEvents.Add(eventInfo);
            }
        }

        private static void ValidateEvent(EventInfo eventInfo, EventInputMemberType type)
        {
            switch (type)
            {
                case EventInputMemberType.Simple:
                    if (eventInfo.EventHandlerType != typeof(EventHandler))
                        throw new InvalidOperationException(eventInfo + " must use a simple " + nameof(EventHandler) + "!");
                    break;
                case EventInputMemberType.Activation:
                    if (eventInfo.EventHandlerType != typeof(EventHandler<ActivationInputEventArgs>))
                        throw new InvalidOperationException(eventInfo + " must use a " + nameof(EventHandler) + " with " + nameof(ActivationInputEventArgs) + "!");
                    break;
                case EventInputMemberType.Axis:
                    if (eventInfo.EventHandlerType != typeof(EventHandler<AxisInputEventArgs>))
                        throw new InvalidOperationException(eventInfo + " must use a " + nameof(EventHandler) + " with " + nameof(AxisInputEventArgs) + "!");
                    break;
                case EventInputMemberType.Axis2D:
                    if (eventInfo.EventHandlerType != typeof(EventHandler<Axis2DInputEventArgs>))
                        throw new InvalidOperationException(eventInfo + " must use a " + nameof(EventHandler) + " with " + nameof(Axis2DInputEventArgs) + "!");
                    break;
                default:
                    throw new NotImplementedException(type.ToString());
            }
        }

        private void ReadMethodReflectionData()
        {
            foreach (var methodInfo in typeof(T).GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                var attribute = methodInfo.GetCustomAttribute<EventInputMemberAttribute>();
                if (attribute == null)
                    continue;

                if (!_data.ContainsKey(attribute.Identifier))
                {
                    _data.Add(attribute.Identifier, new InputReflectionData(attribute.Identifier, attribute.Type));
                }

                ValidateMethod(methodInfo, attribute.Type);
                ValidateData(attribute.Identifier, attribute.Type);

                _data[attribute.Identifier].CheckMethods.Add(methodInfo);
            }
        }

        private static void ValidateMethod(MethodInfo methodInfo, EventInputMemberType type)
        {
            switch (type)
            {
                case EventInputMemberType.Simple:
                case EventInputMemberType.Activation:
                    if (methodInfo.ReturnType != typeof(bool))
                        throw new InvalidOperationException(methodInfo + " must return a boolean value!");
                    break;
                case EventInputMemberType.Axis:
                    if (methodInfo.ReturnType != typeof(float))
                        throw new InvalidOperationException(methodInfo + " must return a float value!");
                    break;
                case EventInputMemberType.Axis2D:
                    if (methodInfo.ReturnType != typeof(Vector2))
                        throw new InvalidOperationException(methodInfo + " must return a Vector2 value!");
                    break;
                default:
                    throw new NotImplementedException(type.ToString());
            }
        }

        private void ValidateData(string identifier, EventInputMemberType type)
        {
            if (_data[identifier].Type != type)
                throw new InvalidOperationException("Found identifier " + identifier + " with multiple types");
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Event)]
    public class EventInputMemberAttribute : Attribute
    {
        public string Identifier { get; }
        public EventInputMemberType Type { get; set; } = EventInputMemberType.Simple;

        public EventInputMemberAttribute(string identifier)
        {
            Identifier = identifier;
        }
    }

    /// <summary>
    /// Type of input for this member
    /// </summary>
    public enum EventInputMemberType
    {
        /// <summary>
        /// Simple input. Is fired if input was detected. Use a simple <see cref="EventHandler"/>
        /// </summary>
        Simple,

        /// <summary>
        /// Input for activation. Fires if state is changed. Use <see cref="EventHandler"/> with <see cref="ActivationInputEventArgs"/>
        /// </summary>
        Activation,

        /// <summary>
        /// Input for an axis. Contains a float value. Use <see cref="EventHandler"/> with <see cref="AxisInputEventArgs"/>
        /// </summary>
        Axis,

        /// <summary>
        /// Input for a 2D axis. Contains a <see cref="Vector2"/> value. Use <see cref="EventHandler"/> with <see cref="Axis2DInputEventArgs"/>
        /// </summary>
        Axis2D,
    }

    internal sealed class InputReflectionData
    {
        public string Identifier { get; }
        public EventInputMemberType Type { get; }
        public IList<MethodInfo> CheckMethods { get; } = new List<MethodInfo>();
        public IList<EventInfo> RiseEvents { get; } = new List<EventInfo>();

        public InputReflectionData(string identifier, EventInputMemberType type)
        {
            Identifier = identifier;
            Type = type;
        }
    }
}
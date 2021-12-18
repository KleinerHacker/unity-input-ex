using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components
{
    public abstract partial class EventInputSystem<T, TS>
    {
        private InputReflectionData[] ReadReflectionData()
        {
            var data = new Dictionary<string, InputReflectionData>();
            
            ReadMethodReflectionData(data);
            ReadPropertyReflectionData(data);
            ReadEventReflectionData(data);

            return data.Values.OrderBy(x => x.Order).ToArray();
        }

        private void ReadEventReflectionData(Dictionary<string, InputReflectionData> data)
        {
            foreach (var eventInfo in GetType().GetEvents())
            {
                var attribute = eventInfo.GetCustomAttribute<EventInputMemberAttribute>();
                if (attribute == null)
                    continue;

                if (!data.ContainsKey(attribute.Identifier))
                {
                    data.Add(attribute.Identifier, new InputReflectionData(attribute.Identifier, attribute.Type, attribute.Order));
                }

                ValidateEvent(eventInfo, attribute.Type);
                ValidateData(attribute.Identifier, attribute.Type, data);

                data[attribute.Identifier].RiseEvents.Add(eventInfo);
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
                case EventInputMemberType.Point:
                    if (eventInfo.EventHandlerType != typeof(EventHandler<Axis2DInputEventArgs>))
                        throw new InvalidOperationException(eventInfo + " must use a " + nameof(EventHandler) + " with " + nameof(Axis2DInputEventArgs) + "!");
                    break;
                default:
                    throw new NotImplementedException(type.ToString());
            }
        }

        private void ReadMethodReflectionData(Dictionary<string, InputReflectionData> data)
        {
            foreach (var methodInfo in typeof(T).GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                var attribute = methodInfo.GetCustomAttribute<EventInputMemberAttribute>();
                if (attribute == null)
                    continue;

                if (!data.ContainsKey(attribute.Identifier))
                {
                    data.Add(attribute.Identifier, new InputReflectionData(attribute.Identifier, attribute.Type, attribute.Order));
                }

                ValidateMethod(methodInfo, attribute.Type);
                ValidateData(attribute.Identifier, attribute.Type, data);

                data[attribute.Identifier].CheckMethods.Add(methodInfo);
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
                case EventInputMemberType.Point:
                    if (methodInfo.ReturnType != typeof(Vector2?))
                        throw new InvalidOperationException(methodInfo + " must return a Vector2? value!");
                    break;
                default:
                    throw new NotImplementedException(type.ToString());
            }
        }

        private void ReadPropertyReflectionData(Dictionary<string, InputReflectionData> data)
        {
            foreach (var propertyInfo in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var attribute = propertyInfo.GetCustomAttribute<EventInputMemberAttribute>();
                if (attribute == null)
                    continue;

                if (!data.ContainsKey(attribute.Identifier))
                {
                    data.Add(attribute.Identifier, new InputReflectionData(attribute.Identifier, attribute.Type, attribute.Order));
                }

                ValidateProperty(propertyInfo, attribute.Type);
                ValidateData(attribute.Identifier, attribute.Type, data);

                data[attribute.Identifier].CheckMethods.Add(propertyInfo.GetMethod);
            }
        }

        private static void ValidateProperty(PropertyInfo propertyInfo, EventInputMemberType type)
        {
            switch (type)
            {
                case EventInputMemberType.Simple:
                case EventInputMemberType.Activation:
                    if (propertyInfo.PropertyType != typeof(bool))
                        throw new InvalidOperationException(propertyInfo + " must return a boolean value!");
                    break;
                case EventInputMemberType.Axis:
                    if (propertyInfo.PropertyType != typeof(float))
                        throw new InvalidOperationException(propertyInfo + " must return a float value!");
                    break;
                case EventInputMemberType.Axis2D:
                    if (propertyInfo.PropertyType != typeof(Vector2))
                        throw new InvalidOperationException(propertyInfo + " must return a Vector2 value!");
                    break;
                case EventInputMemberType.Point:
                    if (propertyInfo.PropertyType != typeof(Vector2?))
                        throw new InvalidOperationException(propertyInfo + " must return a Vector2? value!");
                    break;
                default:
                    throw new NotImplementedException(type.ToString());
            }
        }

        private void ValidateData(string identifier, EventInputMemberType type, Dictionary<string, InputReflectionData> data)
        {
            if (data[identifier].Type != type)
                throw new InvalidOperationException("Found identifier " + identifier + " with multiple types");
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Event | AttributeTargets.Property)]
    public class EventInputMemberAttribute : Attribute
    {
        public string Identifier { get; }
        public EventInputMemberType Type { get; set; } = EventInputMemberType.Simple;
        public int Order { get; set; }

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

        /// <summary>
        /// Input for a single point. This is not a permanently value. Use <see cref="EventHandler"/> with <see cref="AxisInputEventArgs"/>
        /// </summary>
        Point,
    }

    internal sealed class InputReflectionData
    {
        public string Identifier { get; }
        public EventInputMemberType Type { get; }
        public int Order { get; }
        public IList<MethodInfo> CheckMethods { get; } = new List<MethodInfo>();
        public IList<EventInfo> RiseEvents { get; } = new List<EventInfo>();

        public InputReflectionData(string identifier, EventInputMemberType type, int order)
        {
            Identifier = identifier;
            Type = type;
            Order = order;
        }
    }
}
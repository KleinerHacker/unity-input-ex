using System;
using UnityEngine.InputSystem;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Assets;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils.Extensions
{
    public static class InputTypeExtensions
    {
        public static InputDevice GetFitDevice(this InputType type)
        {
            return type switch
            {
                InputType.Keyboard => Keyboard.current,
                InputType.Mouse => Mouse.current,
                InputType.Pointer => Pointer.current,
                InputType.Touchscreen => Touchscreen.current,
                InputType.Gamepad => Gamepad.current,
                _ => throw new NotImplementedException()
            };
        }
        
        public static Type GetFitType(this InputType type)
        {
            return type switch
            {
                InputType.Keyboard => typeof(Keyboard),
                InputType.Mouse => typeof(Mouse),
                InputType.Pointer => typeof(Pointer),
                InputType.Touchscreen => typeof(Touchscreen),
                InputType.Gamepad => typeof(Gamepad),
                _ => throw new NotImplementedException()
            };
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Types;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils.Extensions
{
    public static class KeyboardExtensions
    {
        public static Vector2 GetArrows(this Keyboard keyboard)
        {
            return new Vector2(
                (Keyboard.current.leftArrowKey.isPressed ? -1f : 0f) + (Keyboard.current.rightArrowKey.isPressed ? 1f : 0f),
                (Keyboard.current.upArrowKey.isPressed ? -1f : 0f) + (Keyboard.current.downArrowKey.isPressed ? 1f : 0f)
            );
        }
        
        public static Vector2 GetNumpad(this Keyboard keyboard)
        {
            return new Vector2(
                (Keyboard.current.numpad4Key.isPressed ? -1f : 0f) + (Keyboard.current.numpad6Key.isPressed ? 1f : 0f),
                (Keyboard.current.numpad8Key.isPressed ? -1f : 0f) + (Keyboard.current.numpad2Key.isPressed ? 1f : 0f)
            );
        }
        
        public static Vector2 GetWASD(this Keyboard keyboard)
        {
            return new Vector2(
                (Keyboard.current.aKey.isPressed ? -1f : 0f) + (Keyboard.current.dKey.isPressed ? 1f : 0f),
                (Keyboard.current.wKey.isPressed ? -1f : 0f) + (Keyboard.current.sKey.isPressed ? 1f : 0f)
            );
        }

        public static Vector2 GetAxis(this Keyboard keyboard, KeyAxis axis)
        {
            return axis switch
            {
                KeyAxis.Arrows => GetArrows(keyboard),
                KeyAxis.Numpad => GetNumpad(keyboard),
                KeyAxis.WASD => GetWASD(keyboard),
                _ => throw new NotImplementedException(axis.ToString())
            };
        }
    }
}
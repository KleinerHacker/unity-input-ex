using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Types;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils.Extensions
{
    public static class GamepadExtensions
    {
        public static Vector2 GetAxis(this Gamepad gamepad, GamepadAxis axis)
        {
            return axis switch
            {
                GamepadAxis.DPad => gamepad.dpad.ReadValue(),
                GamepadAxis.LeftStick => gamepad.leftStick.ReadValue(),
                GamepadAxis.RightStick => gamepad.rightStick.ReadValue(),
                _ => throw new NotImplementedException(axis.ToString())
            };
        }
    }
}
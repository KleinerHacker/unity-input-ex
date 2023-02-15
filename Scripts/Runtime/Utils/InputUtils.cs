using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils
{
    public static class InputUtils
    {
        public static TResult GetValueFromDevice<TResult, TDevice>(TDevice device, Func<TDevice, TResult> func) where TDevice : InputDevice
        {
            if (device == null || device.deviceId == InputDevice.InvalidDeviceId)
                return default;

            return func(device);
        }

        public static bool IsPointerPressed()
        {
            return
                GetValueFromDevice(Mouse.current, mouse => mouse.leftButton.isPressed) ||
                GetValueFromDevice(Touchscreen.current, touchscreen => touchscreen.primaryTouch.press.isPressed) ||
                GetValueFromDevice(Pen.current, pen => pen.press.isPressed);
        }

        public static bool WasPointerPressedThisFrame()
        {
            return
                GetValueFromDevice(Mouse.current, mouse => mouse.leftButton.wasPressedThisFrame) ||
                GetValueFromDevice(Touchscreen.current, touchscreen => touchscreen.primaryTouch.press.wasPressedThisFrame) ||
                GetValueFromDevice(Pen.current, pen => pen.press.wasPressedThisFrame);
        }

        public static bool WasPointerReleasedThisFrame()
        {
            return
                GetValueFromDevice(Mouse.current, mouse => mouse.leftButton.wasReleasedThisFrame) ||
                GetValueFromDevice(Touchscreen.current, touchscreen => touchscreen.primaryTouch.press.wasReleasedThisFrame) ||
                GetValueFromDevice(Pen.current, pen => pen.press.wasReleasedThisFrame);
        }

        public static Vector2 GetPointerPosition()
        {
            var mousePos = GetValueFromDevice(Mouse.current, mouse => mouse.position.ReadValue());
            if (mousePos != default)
                return mousePos;

            var touchPos = GetValueFromDevice(Touchscreen.current, touchscreen => touchscreen.primaryTouch.position.ReadValue());
            if (touchPos != default)
                return touchPos;

            var penPos = GetValueFromDevice(Pen.current, pen => pen.position.ReadValue());
            if (penPos != default)
                return penPos;

            return Vector2.zero;
        }

        public static Vector2 GetPointerDelta()
        {
            var mousePos = GetValueFromDevice(Mouse.current, mouse => mouse.delta.ReadValue());
            if (mousePos != default)
                return mousePos;

            var touchPos = GetValueFromDevice(Touchscreen.current, touchscreen => touchscreen.primaryTouch.delta.ReadValue());
            if (touchPos != default)
                return touchPos;

            var penPos = GetValueFromDevice(Pen.current, pen => pen.delta.ReadValue());
            if (penPos != default)
                return penPos;

            return Vector2.zero;
        }
    }
}
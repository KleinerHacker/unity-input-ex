using System;
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
    }
}
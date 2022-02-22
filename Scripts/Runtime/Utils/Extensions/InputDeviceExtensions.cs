using UnityEngine.InputSystem;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils.Extensions
{
    public static class InputDeviceExtensions
    {
        public static bool IsAvailable(this InputDevice inputDevice) => 
            inputDevice != null && inputDevice.deviceId != InputDevice.InvalidDeviceId && inputDevice.enabled;
    }
}
using System.Collections.Generic;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Assets;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Types;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Utils.Extensions
{
    public static class InputActionReferenceExtensions
    {
        private static readonly IDictionary<InputActionReference, InputAction> _actionCache = new Dictionary<InputActionReference, InputAction>();
        
        public static InputAction ToInputAction(this InputActionReference reference)
        {
            if (!_actionCache.ContainsKey(reference))
            {
                _actionCache.Add(reference, new InputAction());
            }

            return _actionCache[reference];
        }

        public static InputAction ToInputAction(this InputActionInfo info)
        {
            return info.Reference.ToInputAction();
        }
    }
}
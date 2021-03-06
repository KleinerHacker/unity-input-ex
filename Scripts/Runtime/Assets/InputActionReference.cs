using System;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Extra;
using UnityEngine;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime.Assets
{
    [CreateAssetMenu(menuName = UnityInputConstants.Root + "/Input Action Reference", fileName = "InputAction")]
    public sealed class InputActionReference : ScriptableObject
    {
        #region Inspector Data

        [SerializeField]
        [ReadOnly]
        private string guid = System.Guid.NewGuid().ToString();

        #endregion

        #region Properties

        public string Guid => guid;

        #endregion

        private bool Equals(InputActionReference other)
        {
            return base.Equals(other) && guid == other.guid;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is InputActionReference other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (guid != null ? guid.GetHashCode() : 0);
            }
        }
    }

    [Serializable]
    public sealed class InputActionInfo
    {
        #region Inspector Data

        [SerializeField]
        private InputActionReference reference;

        [SerializeField]
        private string valueKey;

        #endregion

        #region Properties

        public InputActionReference Reference => reference;

        public string ValueKey => valueKey;

        #endregion
    }
}
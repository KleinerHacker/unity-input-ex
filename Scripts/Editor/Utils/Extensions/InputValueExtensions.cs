using System;
using UnityEngine.InputSystem.Controls;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Assets;

namespace UnityInputEx.Editor.input_ex.Scripts.Editor.Utils.Extensions
{
    internal static class InputValueExtensions
    {
        public static Type GetFitType(this InputValue value)
        {
            return value switch
            {
                InputValue.Button => typeof(ButtonControl),
                InputValue.Float => typeof(AxisControl),
                InputValue.Integer => typeof(IntegerControl),
                InputValue.Vector2 => typeof(Vector2Control),
                _ => throw new NotImplementedException()
            };
        }
    } 
}
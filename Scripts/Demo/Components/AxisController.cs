using UnityEngine;
using UnityEngine.EventSystems;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components;

namespace UnityInputEx.Demo.input_ex.Scripts.Demo.Components
{
    public sealed class AxisController : UIBehaviour
    {
        protected override void OnEnable()
        {
            DemoEventInputSystem.Singleton.OnAxis += OnAxis;
        }
        
        protected override void OnDisable()
        {
            DemoEventInputSystem.Singleton.OnAxis -= OnAxis;
        }

        private void OnAxis(object sender, AxisInputEventArgs e)
        {
            transform.localPosition += Vector3.right * e.Value;
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components;

namespace UnityInputEx.Demo.input_ex.Scripts.Demo.Components
{
    public sealed class Axis2DController : UIBehaviour
    {
        protected override void OnEnable()
        {
            DemoEventInputSystem.Singleton.OnAxis2D += OnAxis2D;
        }
        
        protected override void OnDisable()
        {
            DemoEventInputSystem.Singleton.OnAxis2D -= OnAxis2D;
        }

        private void OnAxis2D(object sender, Axis2DInputEventArgs e)
        {
            transform.localPosition += new Vector3(e.Value.x, e.Value.y, 0f);
        }
    }
}
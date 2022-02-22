#if DEMO
using UnityEngine;
using UnityEngine.EventSystems;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components;

namespace UnityInputEx.Demo.input_ex.Scripts.Demo.Components
{
    public sealed class PointController : UIBehaviour
    {
        protected override void OnEnable()
        {
            DemoEventInputSystem.Singleton.OnPoint += OnPoint;
        }

        protected override void OnDisable()
        {
            DemoEventInputSystem.Singleton.OnPoint -= OnPoint;
        }

        private void OnPoint(object sender, Axis2DInputEventArgs e)
        {
            transform.localPosition += new Vector3(e.Value.x, e.Value.y, 0f);
        }
    }
}
#endif
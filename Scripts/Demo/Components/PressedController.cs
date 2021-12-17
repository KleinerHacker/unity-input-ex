using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityInputEx.Demo.input_ex.Scripts.Demo.Components
{
    public sealed class PressedController : UIBehaviour
    {
        protected override void OnEnable()
        {
            DemoEventInputSystem.Singleton.OnPressed += OnPressed;
        }

        protected override void OnDisable()
        {
            DemoEventInputSystem.Singleton.OnPressed -= OnPressed;
        }

        private void OnPressed(object sender, EventArgs e)
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = canvasGroup.alpha > 0.5f ? 0f : 1f;
        }
    }
}
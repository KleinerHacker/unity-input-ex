using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components;

namespace UnityInputEx.Demo.input_ex.Scripts.Demo.Components
{
    public sealed class ActivationController : UIBehaviour
    {
        protected override void OnEnable()
        {
            DemoEventInputSystem.Singleton.OnActivated += OnActivated;
        }
        
        protected override void OnDisable()
        {
            DemoEventInputSystem.Singleton.OnActivated -= OnActivated;
        }

        private void OnActivated(object sender, ActivationInputEventArgs e)
        {
            GetComponent<CanvasGroup>().alpha = e.Type == ActivationInputEventArgs.ActivationType.Activated ? 1f : 0f;
        }
    }
}
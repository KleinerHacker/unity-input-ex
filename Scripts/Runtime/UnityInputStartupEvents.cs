using System;
using System.Linq;
using UnityEngine;
using UnityInputEx.Runtime.input_ex.Scripts.Runtime.Components;

namespace UnityInputEx.Runtime.input_ex.Scripts.Runtime
{
    public static class UnityInputStartupEvents
    {
        #region Static Area

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void CreateEventInputSystem()
        {
            Debug.Log("Create event input systems");

            var inputSystemTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IEventInputSystem).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToArray();

            foreach (var inputSystemType in inputSystemTypes)
            {
                Debug.Log("> Create " + inputSystemType.Name);
                
                var go = new GameObject("Event Input System (" + inputSystemType.Name + ")");
                go.AddComponent(inputSystemType);
                GameObject.DontDestroyOnLoad(go);
            }
        }

        #endregion
    }
}
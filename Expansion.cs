using System;

using UnityEngine;

namespace UnityExpansion
{
    /// <summary>
    /// Main class of UnityExpansion. Will be automatically initialized when required.
    /// If you are looking any information visit http://okov.se/expansion/
    /// </summary>
    public static class Expansion
    {
        /// <summary>
        /// Invoked on every frame.
        /// </summary>
        public static event Action OnUpdate;

        private static ExpansionComponent _expansionComponent;

        static Expansion()
        {
            if(_expansionComponent == null)
            {
                GameObject gameObject = new GameObject("UnityExpansion");
                
                _expansionComponent = gameObject.AddComponent<ExpansionComponent>();
                _expansionComponent.OnUpdate += OnComponentUpdate;

                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }
        }

        private static void OnComponentUpdate()
        {
            OnUpdate.InvokeIfNotNull();
        }
    }
}
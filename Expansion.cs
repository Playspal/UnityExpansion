using System;

using UnityEngine;

namespace UnityExpansion
{
    /// <summary>
    /// Main class of UnityExpansion and it should be added as component to one of game objects.
    /// If you are looking any information visit http://okov.se/expansion/
    /// </summary>
    [AddComponentMenu("Expansion/Expansion Main Object", 1)]
    public class Expansion : MonoBehaviour
    {
        public const string VERSION = "2.0.0";

        /// <summary>
        /// Invoked on every frame.
        /// </summary>
        public event Action OnUpdate; 

        /// <summary>
        /// Expansion instance.
        /// </summary>
        public static Expansion Instance { get; private set; }

        // Initialization
        private void Awake()
        {
            Instance = this;
        }

        // Updates internal part of UnityExpansion.
        private void Update()
        {
            OnUpdate.InvokeIfNotNull();
        }
    }
}
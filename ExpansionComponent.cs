using System;

using UnityEngine;

namespace UnityExpansion
{
    /// <summary>
    /// Main component that dispatches event on every update.
    /// </summary>
    public class ExpansionComponent : MonoBehaviour
    {
        /// <summary>
        /// Invoked on every update.
        /// </summary>
        public event Action OnUpdate;

        // Main iteration
        private void Update()
        {
            OnUpdate.InvokeIfNotNull();
        }
    }
}
using UnityEngine;
using UnityExpansion.Services;

namespace UnityExpansion.Utilities
{
    public static class UtilityTime
    {
        /// <summary>
        /// The time in seconds it took to complete the last frame.
        /// Clamped between 0 to 1.
        /// </summary>
        public static float DeltaTime
        {
            get
            {
                return Mathf.Min(Time.deltaTime, 1);
            }
        }

        /// <summary>
        /// Frames per second.
        /// </summary>
        public static float Fps = 0;

        // Used to calculate Fps
        private static float _deltaTimeAccumulator = 0;

        /// <summary>
        /// Initializes the UtilityTime class.
        /// </summary>
        static UtilityTime()
        {
            Expansion.OnUpdate += Update;
        }

        // Updates fps
        private static void Update()
        {
            _deltaTimeAccumulator += (Time.deltaTime - _deltaTimeAccumulator) * 0.1f;

            if (_deltaTimeAccumulator > 0)
            {
                Fps = 1.0f / _deltaTimeAccumulator;
            }
        }
    }
}
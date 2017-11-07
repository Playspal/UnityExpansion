using UnityEngine;
using UnityExpansion.Services;

namespace UnityExpansion
{
    public static class UtilityTime
    {
        /// <summary>
        /// The time in seconds it took to complete the last frame.
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
            Singnals.AddListener(UnityExpansionIndex.SIGNAL_FRAME_START, UpdateFps);
        }



        private static void UpdateFps()
        {
            _deltaTimeAccumulator += (Time.deltaTime - _deltaTimeAccumulator) * 0.1f;

            Fps = 1.0f / _deltaTimeAccumulator;
        }
    }
}
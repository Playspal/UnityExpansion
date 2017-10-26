using UnityExpansion.Services;

namespace UnityExpansion
{
    public static class UnityExpansion
    {
        public const string SIGNAL_FRAME_START = "UnityExpansion/FrameStart";

        /// <summary>
        /// Update internal part of UnityExpansion
        /// </summary>
        public static void Update()
        {
            Singnals.Dispatch(SIGNAL_FRAME_START);
        }

        public static void ExecuteInMainThread()
        {

        }
    }
}
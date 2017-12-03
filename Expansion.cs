using UnityEngine;
using UnityExpansion.Services;
using UnityExpansion.UI;

namespace UnityExpansion
{
    /// <summary>
    /// Main class of UnityExpansion and it should be added as component to one of game objects.
    /// </summary>
    [AddComponentMenu("Expansion/Expansion Main Object", 1)]
    public class Expansion : MonoBehaviour
    {
        /// <summary>
        /// Signal name that dispatched on start of each frame.
        /// </summary>
        public const string SIGNAL_FRAME_START = "UnityExpansion/FrameStart";

        /// <summary>
        /// Expansion instance.
        /// </summary>
        public static Expansion Instance { get; private set; }

        [SerializeField]
        public UiLayoutSettings LayoutSettings;

        // Initialization
        private void Awake()
        {
            Instance = this;
        }

        // Updates internal part of UnityExpansion.
        private void Update()
        {
            Signals.Dispatch(SIGNAL_FRAME_START);
        }

        // Resets UnityExpansion component to default state.
        private void Reset()
        {
            if(LayoutSettings == null)
            {
                LayoutSettings = gameObject.GetOrAddComponent<UiLayoutSettings>();
                LayoutSettings.hideFlags = HideFlags.HideInInspector;
            }
        }
    }
}
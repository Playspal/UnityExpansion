using UnityEngine;
using UnityExpansion.Services;
using UnityExpansion.UI;
using UnityExpansionInternal;

namespace UnityExpansion
{
    /// <summary>
    /// Main class of UnityExpansion and it should be added as component to one of game objects.
    /// </summary>
    [AddComponentMenu("Expansion/Expansion Main Object", 1)]
    public class Expansion : MonoBehaviour
    {
        public const string VERSION = "1.1.5";

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

        [SerializeField]
        public InternalSettings InternalSettings;

        // Initialization
        private void Awake()
        {
            Debug.Log("Initializated UnityExpansion " + VERSION + ". If you are looking any information visit http://okov.se/expansion/");
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

            if(InternalSettings == null)
            {
                InternalSettings = gameObject.GetOrAddComponent<InternalSettings>();
                InternalSettings.hideFlags = HideFlags.HideInInspector;
            }
        }
    }
}
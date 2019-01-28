using System;
using System.Collections.Generic;

using UnityEngine;

namespace UnityExpansion.UI
{
    /// <summary>
    /// UiLayoutPresets manager. Used as component to setup layout elements in inspector.
    /// UnityExpansion main object always have one instance of UiLayoutSettings,
    /// so usually you don't need to use this class manualy.
    /// </summary>
    /// <seealso cref="UnityExpansion.UI.UiLayout" />
    /// <seealso cref="UnityExpansion.UI.UiLayoutPreset" />
    [Serializable]
    public class UiLayoutSettings : MonoBehaviour
    {
        /// <summary>
        /// List of screens presets.
        /// </summary>
        [SerializeField]
        public List<UiLayoutPreset> Screens = new List<UiLayoutPreset>();

        /// <summary>
        /// List of panels presets.
        /// </summary>
        [SerializeField]
        public List<UiLayoutPreset> Panels = new List<UiLayoutPreset>();

        /// <summary>
        /// List of popups presets.
        /// </summary>
        [SerializeField]
        public List<UiLayoutPreset> Popups = new List<UiLayoutPreset>();

        /// <summary>
        /// Default container.
        /// </summary>
        [SerializeField]
        public RectTransform DefaultContainer;

        // Initializates instance
        void Start()
        {
            if (DefaultContainer != null)
            {
                UiLayout.DefaultContainer = DefaultContainer;
            }

            InitializeList(Screens, UiLayoutPreset.Type.Screen);
            InitializeList(Panels, UiLayoutPreset.Type.Panel);
            InitializeList(Popups, UiLayoutPreset.Type.Popup);
        }

        // Initializates list items
        private void InitializeList(List<UiLayoutPreset> list, UiLayoutPreset.Type type)
        {
            for (int i = 0; i < list.Count; i++)
            {
                UiLayoutPreset item = list[i];

                if (item != null && !string.IsNullOrEmpty(item.PrefabPath))
                {
                    item.PresetType = type;
                    UiLayout.InitializePreset(item);
                }
            }
        }
    }
}
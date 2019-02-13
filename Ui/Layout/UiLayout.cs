using System;
using System.Collections.Generic;

using UnityEngine;

namespace UnityExpansion.UI
{
    [Serializable]
    public class UiLayout : UiObject
    {
        /// <summary>
        /// Signals that will be dispatched when layout will be created or enabled.
        /// </summary>
        [SerializeField]
        public string[] SignalsOnEnable = new string[0];

        /// <summary>
        /// List of layout elements attached to this layout.
        /// </summary>
        [SerializeField]
        public UiLayoutPreset[] Presets = new UiLayoutPreset[0];

        public void AddPreset(UiLayoutPreset preset)
        {
            Array.Resize<UiLayoutPreset>(ref Presets, Presets.Length + 1);
            Presets[Presets.Length - 1] = preset;
        }
    }
}
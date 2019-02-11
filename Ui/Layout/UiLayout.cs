using System;
using System.Collections.Generic;

using UnityEngine;

namespace UnityExpansion.UI
{
    [Serializable]
    public class UiLayout : UiObject
    {
        /// <summary>
        /// List of layout elements attached to this layout.
        /// </summary>
        [SerializeField]
        public List<UiLayoutPreset> Presets = new List<UiLayoutPreset>();
    }
}
using System;
using UnityEngine;

namespace UnityExpansion.UI
{
    [Serializable]
    public class UiLayoutPreset
    {
        /// <summary>
        /// Full path to original prefab in Assets folder.
        /// </summary>
        public string AssetPath;

        /// <summary>
        /// Path to original prefab in Resources folder.
        /// </summary>
        public string PrefabPath;

        /// <summary>
        /// Instansiated prefab.
        /// </summary>
        public UiLayoutElement Instance { get; private set; }

        /// <summary>
        /// Preset position in UiLayoutEditor screen.
        /// </summary>
        public Vector2 EditorPosition = new Vector2();
    }
}
using System;
using UnityEngine;

namespace UnityExpansion.UI
{
    [Serializable]
    public class UiLayoutPreset
    {
        /// <summary>
        /// Loaded prefab.
        /// </summary>
        public UiLayoutElement Prefab;

        /// <summary>
        /// Instansiated prefab.
        /// </summary>
        public UiLayoutElement Instance { get; private set; }

        /// <summary>
        /// Instantiates prefab.
        /// </summary>
        /// <param name="parent"></param>
        public void Instantiate(RectTransform parent)
        {
            Instance = GameObject.Instantiate(Prefab, parent);
        }
    }
}
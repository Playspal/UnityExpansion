using System;

using UnityEngine;

namespace UnityExpansion.UI.Layout.Processor
{
    /// <summary>
    /// TODO: write description
    /// </summary>
    [Serializable]
    public class UiLayoutProcessorPreset
    {
        /// <summary>
        /// The prefab of UiLayoutElement.
        /// </summary>
        [SerializeField]
        public UiLayoutElement Prefab;

        /// <summary>
        /// Parent container.
        /// </summary>
        [SerializeField]
        public RectTransform Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiLayoutProcessorPreset"/> class.
        /// </summary>
        public UiLayoutProcessorPreset(UiLayoutElement prefab)
        {
            Prefab = prefab;
        }

        /// <summary>
        /// Instantiates UiLayoutElement.
        /// </summary>
        public UiLayoutElement Instantiate()
        {
            UiLayoutElement instance = GameObject.Instantiate(Prefab, Container);

            instance.SetActive(false);

            return instance;
        }
    }
}
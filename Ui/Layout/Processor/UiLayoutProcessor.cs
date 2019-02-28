using System;
using System.Collections.Generic;

using UnityEngine;

namespace UnityExpansion.UI.Layout.Processor
{
    /// <summary>
    /// TODO: Write description
    /// </summary>
    [Serializable]
    public class UiLayoutProcessor
    {
        /// <summary>
        /// List of registered presets.
        /// </summary>
        [SerializeField]
        public List<UiLayoutProcessorPreset> Presets = new List<UiLayoutProcessorPreset>();

        /// <summary>
        /// List of registered edicts.
        /// </summary>
        [SerializeField]
        public List<UiLayoutProcessorEdict> Edicts = new List<UiLayoutProcessorEdict>();

        public void Setup(UiLayout layout)
        {
            // Setup provided layout
            SetupElement(layout);

            // Instantiate and setup presets
            for (int i = 0; i < Presets.Count; i++)
            {
                UiLayoutElement element = Presets[i].Instantiate();
                SetupElement(element);
            }
        }

        private void SetupElement(UiLayoutElement layoutElement)
        {
            // Find all objects with PersistantID and assign them to edicts.
            PersistantIDExplorer.Explore
            (
                layoutElement.gameObject,
                SetupObject
            );
        }

        private void SetupObject(object target, PersistantID id)
        {
            for(int i = 0; i < Edicts.Count; i++)
            {
                UiLayoutProcessorEdict edict = Edicts[i];

                if (edict.SenderID == id)
                {
                    edict.SetSenderObject(target);
                }

                if (edict.HandlerID == id)
                {
                    edict.SetHandlerObject(target);
                }
            }
        }
    }
}
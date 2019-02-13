using System;
using System.Collections.Generic;

using UnityEngine;
using UnityExpansion.Services;

namespace UnityExpansion.UI
{
    [Serializable]
    public class UiLayout : UiObject
    {
        public Signal SignalOnEnable = new Signal("__uiLayoutOnEnable");

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

        protected override void Start()
        {
            Debug.LogError("Start");
            base.Start();

            for(int i = 0; i < Presets.Length; i++)
            {
                SetupPreset(Presets[i]);
            }

            Debug.LogError("OnEnable");
            Signals.Dispatch("__uiLayoutOnEnable");
        }

        private void SetupPreset(UiLayoutPreset preset)
        {
            GameObject gameObject = Resources.Load<GameObject>(preset.PrefabPath);
            UiLayoutElement layoutElement = gameObject.GetComponent<UiLayoutElement>();

            for (int n = 0; n < layoutElement.SignalsShow.Length; n++)
            {
                Debug.LogError("!!!" + layoutElement.SignalsShow[n] + " / " + gameObject);

                Signals.AddListener
                (
                    layoutElement.SignalsShow[n],
                    () =>
                    {
                        Debug.LogError("OnSignal");
                        Instantiate<UiLayoutElement>(layoutElement, RectTransform);
                    }
                );
            }
        }
    }
}
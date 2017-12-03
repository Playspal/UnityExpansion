using System;

using UnityEngine;
using UnityExpansion.Services;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Simple controller for UiLayoutElements that provides registration in UiLayout, subscribing to signals and attaching to specified container.
    /// It used in UiLayoutSettings component to setup layout elements in inspector so usually you don't need to use this class manualy.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityExpansion.Services;
    /// using UnityExpansion.UI;
    /// 
    /// public class MyClass
    /// {
    ///     public MyClass()
    ///     {
    ///         RectTransform container = GameObject.Find("Canvas").GetComponent<RectTransform>();
    /// 
    ///         UiLayoutPreset preset = new UiLayoutPreset
    ///         {
    ///             PresetType = UiLayoutPreset.Type.Popup,
    ///             PrefabPath = "Interfaces/MyPopup",
    ///             Container = container,
    ///             SignalShow = "MyPopup.Show",
    ///             SignalHide = "MyPopup.Hide"
    ///         };
    ///         
    ///         UiLayout.InitializePreset(preset);
    ///         
    ///         // Creates and shows first popup by signal
    ///         Signals.Dispatch("MyPopup.Show");
    ///         
    ///         // Creates and shows second popup by prefab path in Resources folder
    ///         UiLayoutElementPopup popup = UiLayout.CreatePopup("Interfaces/MyPopup");
    ///         
    ///         // Hides and destroys both popups
    ///         Signals.Dispatch("MyPopup.Hide");
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="UnityExpansion.UI.UiLayout" />
    /// <seealso cref="UnityExpansion.UI.UiLayoutSettings" />
    [Serializable]
    public class UiLayoutPreset
    {
        /// <summary>
        /// Presents the type of layout element.
        /// </summary>
        public enum Type
        {
            Screen,
            Panel,
            Popup
        }

        /// <summary>
        /// Type of preset.
        /// </summary>
        public Type PresetType;

        /// <summary>
        /// Path to original prefab in Resources folder.
        /// </summary>
        public string PrefabPath;

        /// <summary>
        /// Instansiated prefab.
        /// </summary>
        public UiLayoutElement Instance { get; private set; }

        /// <summary>
        /// Parent container.
        /// </summary>
        public RectTransform Container;

        /// <summary>
        /// Signal to show preset's instance
        /// </summary>
        public string SignalShow;

        /// <summary>
        /// Signal to hide preset's instance
        /// </summary>
        public string SignalHide;

        /// <summary>
        /// Is the preset will be active at start.
        /// </summary>
        public bool ActiveByDefault = false;

        /// <summary>
        /// Is the preset will be active at start without any delays or playing animations.
        /// </summary>
        public bool ActiveByDefaultImmediately = false;

        /// <summary>
        /// Initializates layout preset.
        /// </summary>
        public void Initialization()
        {
            switch (PresetType)
            {
                case Type.Screen:
                    Instance = UiLayout.CreateScreen(PrefabPath, Container);
                    Instance.SetActive(false);

                    if (ActiveByDefault)
                    {
                        if (ActiveByDefaultImmediately)
                        {
                            UiLayout.SetActiveScreenImmediately(Instance as UiLayoutElementScreen);
                        }
                        else
                        {
                            UiLayout.SetActiveScreen(Instance as UiLayoutElementScreen);
                        }
                    }
                    break;

                case Type.Panel:
                    Instance = UiLayout.CreatePanel(PrefabPath, Container);
                    Instance.SetActive(false);

                    if (ActiveByDefault)
                    {
                        if (ActiveByDefaultImmediately)
                        {
                            Instance.ShowImmediately();
                        }
                        else
                        {
                            Instance.Show();
                        }
                    }

                    break;
            }

            Signals.AddListener(SignalShow, OnSignalShow);
            Signals.AddListener(SignalHide, OnSignalHide);
        }

        // Signal show handler
        private void OnSignalShow()
        {
            switch (PresetType)
            {
                case Type.Screen:
                    UiLayout.SetActiveScreen(Instance as UiLayoutElementScreen);
                    break;

                case Type.Panel:
                    Instance.Show();
                    break;

                case Type.Popup:
                    UiLayoutElementPopup popup = UiLayout.CreatePopup(PrefabPath, Container);
                    Signals.AddListener(SignalHide, popup.Hide);
                    break;
            }
        }

        // Signal hide handler
        private void OnSignalHide()
        {
            switch (PresetType)
            {
                case Type.Screen:
                case Type.Panel:
                    Instance.Hide();
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityExpansion.Services;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Static class that provides general ui management. Creates screens, panels and popups manually or using UiLayoutPresets.
    /// Controlls currently active screen, provides transitions between screens.
    /// </summary>
    /// <seealso cref="UnityExpansion.UI.UiLayoutPreset" />
    /// <seealso cref="UnityExpansion.UI.UiLayoutSettings" />
    public static class UiLayout
    {
        /// <summary>
        /// Default container to place layout elements.
        /// </summary>
        public static RectTransform DefaultContainer;

        /// <summary>
        /// Currently active screen.
        /// </summary>
        public static UiLayoutElementScreen ActiveScreen { get; private set; }

        // List of items that was seted in editor mode
        private static List<UiLayoutPreset> _presets = new List<UiLayoutPreset>();

        // List of instantiated elements
        private static List<CommonPair<string, UiLayoutElement>> _elements = new List<CommonPair<string, UiLayoutElement>>();

        // Currently active transition
        private static UiLayoutTransition _activeTransition;

        /// <summary>
        /// Immediately sets the active screen without playing any hide and show animations.
        /// </summary>
        /// <param name="screen">Target screen</param>
        public static void SetActiveScreenImmediately(UiLayoutElementScreen screen)
        {
            if (ActiveScreen != null)
            {
                ActiveScreen.HideImmediately();
            }

            ActiveScreen = screen;

            if (ActiveScreen != null)
            {
                ActiveScreen.ShowImmediately();
            }
        }

        /// <summary>
        /// Sets and shows the active screen and hides previous active screen.
        /// </summary>
        /// <param name="screen">Target screen</param>
        /// <param name="transitionOffset">
        /// Normalized time of current screen's hide duration that the target screen will wait before start to showing.
        /// For example, a value of 0.5 would mean the target screen will begin showing at 50% of the current screen hide animation time.
        /// </param>
        public static void SetActiveScreen(UiLayoutElementScreen screen, float transitionOffset = .5f)
        {
            if (_activeTransition != null)
            {
                return;
            }

            _activeTransition = new UiLayoutTransition
            (
                ActiveScreen,
                screen,
                transitionOffset
            );

            _activeTransition.OnHide += () =>
            {
                ActiveScreen = screen;

                _activeTransition = null;
            };

            _activeTransition.OnComplete += () =>
            {
                ActiveScreen = screen;

                _activeTransition = null;
            };

            _activeTransition.Start();
        }

        /// <summary>
        /// Searches UiLayoutElementScreen that was loaded from specified path in Resources folder.
        /// </summary>
        /// <param name="path">Original prefab path in a Resources folder</param>
        /// <returns>UiLayoutElementScreen instance or null if not found</returns>
        public static UiLayoutElementScreen FindScreen(string path)
        {
            UiLayoutElement element = GetElement(path);

            if (element != null && element is UiLayoutElementScreen)
            {
                return element as UiLayoutElementScreen;
            }

            return null;
        }

        /// <summary>
        /// Searches UiLayoutElementPanel that was loaded from specified path in Resources folder.
        /// </summary>
        /// <param name="path">Original prefab path in a Resources folder</param>
        /// <returns>UiLayoutElementPanel instance or null if not found</returns>
        public static UiLayoutElementPanel FindPanel(string path)
        {
            UiLayoutElement element = GetElement(path);

            if (element != null && element is UiLayoutElementPanel)
            {
                return element as UiLayoutElementPanel;
            }

            return null;
        }

        /// <summary>
        /// Loads and instantiates UiLayoutElementScreen stored at path in a Resources folder.
        /// Throws exception if screen with specified path was instantiated before.
        /// </summary>
        /// <param name="path">Prefab path in a Resources folder</param>
        /// <param name="parent">Parent RectTransform</param>
        /// <returns>Instance of UiLayoutElementScreen</returns>
        public static UiLayoutElementScreen CreateScreen(string path, RectTransform parent = null)
        {
            if (GetElement(path) != null)
            {
                throw new Exception("Screen " + path + " is already instantiated. Use UiLayout.FindScreen to get the instance.");
            }

            UiLayoutElementScreen screen = UiObject.Instantiate<UiLayoutElementScreen>(path, parent ?? DefaultContainer);

            AddElement(path, screen);

            return screen;
        }

        /// <summary>
        /// Loads and instantiates UiLayoutElementPanel stored at path in a Resources folder.
        /// Throws exception if panel with specified path was instantiated before.
        /// </summary>
        /// <param name="path">Prefab path in a Resources folder</param>
        /// <param name="parent">Parent RectTransform</param>
        /// <returns>Instance of UiLayoutElementPanel</returns>
        public static UiLayoutElementPanel CreatePanel(string path, RectTransform parent = null)
        {
            if (GetElement(path) != null)
            {
                throw new Exception("Panel " + path + " is already instantiated. Use UiLayout.FindPanel to get the instance.");
            }

            UiLayoutElementPanel panel = UiObject.Instantiate<UiLayoutElementPanel>(path, parent ?? DefaultContainer);

            AddElement(path, panel);

            return panel;
        }

        /// <summary>
        /// Loads, instantiates and shows UiLayoutElementPopup stored at path in a Resources folder.
        /// If popup with specified path was setted up by using UiLayoutSettings in inspector or by using UiLayoutPreset
        /// then it will be created with assigned parameters like parent container and signals to show and hide.
        /// </summary>
        /// <param name="path">Prefab path in a Resources folder</param>
        /// <param name="parent">Parent RectTransform. Leave it null to use default container or container that assigned to UiLayoutPreset.</param>
        /// <returns>Instance of UiLayoutElementPanel</returns>
        public static UiLayoutElementPopup CreatePopup(string path, RectTransform parent = null)
        {
            UiLayoutPreset preset = FindPreset(path);

            if (preset != null)
            {
                parent = parent ?? preset.Container;
            }

            UiLayoutElementPopup popup = UiObject.Instantiate<UiLayoutElementPopup>(path, parent ?? DefaultContainer);

            if(preset != null)
            {
                Signals.AddListener(preset.SignalHide, popup.Hide);
            }

            return popup;
        }

        /// <summary>
        /// Initializes UiLayoutPreset that describes behavior of layout element.
        /// </summary>
        /// <param name="preset">The preset</param>
        public static void InitializePreset(UiLayoutPreset preset)
        {
            preset.Initialization();

            _presets.Add(preset);
        }

        // Searches preset by specified path
        private static UiLayoutPreset FindPreset(string path)
        {
            for (int i = 0; i < _presets.Count; i++)
            {
                if (_presets[i].PrefabPath == path)
                {
                    return _presets[i];
                }
            }

            return null;
        }

        // Searches layout element by specified path
        private static UiLayoutElement GetElement(string path)
        {
            CommonPair<string, UiLayoutElement> pair = _elements.Find(x => x.Key == path);

            if(pair != null)
            {
                return pair.Value;
            }

            return null;
        }

        // Adds new layout element to list
        private static void AddElement(string path, UiLayoutElement element)
        {
            if (GetElement(path) != null)
            {
                return;
            }

            _elements.Add(new CommonPair<string, UiLayoutElement>(path, element));

            Reorder();
        }

        // Reorders depth of layout element inside of theyr containers.
        private static void Reorder()
        {
            for(int index = 0; index < _elements.Count; index++)
            {
                if (_elements[index] == null || _elements[index].Value == null || _elements[index].Value.IsDestroyed)
                {
                    continue;
                }

                UiLayoutElement element = _elements[index].Value;

                int depth = 0;

                if (element is UiLayoutElementScreen)
                {
                    depth = index + (element.IsActive ? 100 : 150);
                }

                if (element is UiLayoutElementPanel)
                {
                    depth = index + (element.IsActive ? 200 : 250);
                }

                if (element is UiLayoutElementPopup)
                {
                    depth = index + (element.IsActive ? 300 : 350);
                }

                element.RectTransform.SetSiblingIndex(depth);

                index++;
            }
        }
    }
}
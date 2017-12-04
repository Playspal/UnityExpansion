#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityExpansion;
using UnityExpansion.UI;

namespace UnityExpansionInternal
{
    [CustomEditor(typeof(UiLayoutSettings))]
    public class InternalUiLayoutEditor : Editor
    {
        private enum ItemType
        {
            Screen,
            Panel,
            Popup
        }

        private UiLayoutPreset _dynamicItemSelected = null;
        private UiLayoutPreset _staticItemSelected = null;
        private UiLayoutPreset _popupItemSelected = null;

        public override void OnInspectorGUI()
        {
            UiLayoutSettings layoutSettings = target as UiLayoutSettings;

            GUIStyle style = new GUIStyle(GUI.skin.GetStyle("HelpBox"));
            style.padding = new RectOffset(7, 7, 7, 7);

            GUILayout.BeginVertical(style);
            layoutSettings.DefaultContainer = (RectTransform)EditorGUILayout.ObjectField("Default Container", layoutSettings.DefaultContainer, typeof(RectTransform), allowSceneObjects: true);
            GUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Screens Presets");
            _dynamicItemSelected = RenderList(layoutSettings.Screens, _dynamicItemSelected, ItemType.Screen);
            
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Panels Presets");
            _staticItemSelected = RenderList(layoutSettings.Panels, _staticItemSelected, ItemType.Panel);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Popups Presets");
            _popupItemSelected = RenderList(layoutSettings.Popups, _popupItemSelected, ItemType.Popup);

            EditorGUILayout.Space();
        }

        private UiLayoutPreset RenderList(List<UiLayoutPreset> list, UiLayoutPreset listItemSelected, ItemType type)
        {
            UiLayoutSettings layoutSettings = target as UiLayoutSettings;

            GUILayout.BeginVertical(new GUIStyle(GUI.skin.GetStyle("HelpBox")));

            if (list.Count == 0)
            {
                switch (type)
                {
                    case ItemType.Screen:
                        EditorGUILayout.HelpBox("\nScreens is a basic layout elements like Main Menu, Choosing of level, Settings, etc. Only one slide can be active at the same time so if new slide shown then the old one will be automatically hidden. \n", MessageType.Info);
                        break;

                    case ItemType.Panel:
                        EditorGUILayout.HelpBox("\nPanels is an additional layout elements like header, footer, etc. Panels can be shown and hided separatelly at any time. \n", MessageType.Info);
                        break;

                    case ItemType.Popup:
                        EditorGUILayout.HelpBox("\nPopups is a overlaing layout elements like alerts, confirms and etc. Unlimited amount of same popups can be shown at the same time. After popup is hidden it's instance will be destroyed. \n", MessageType.Info);
                        break;
                }
            }

            // Items
            for (int i = 0; i < list.Count; i++)
            {
                listItemSelected = RenderListItem(list, list[i], i, listItemSelected, type);
            }

            // Buttons
            GUIStyle buttonsStyle = new GUIStyle();
            buttonsStyle.margin.left = -5;// (int)EditorGUIUtility.currentViewWidth - (listItemSelected == null ? 110 : 213);
            
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.GetStyle("HelpBox"));
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.stretchWidth = false;
            buttonStyle.fixedWidth = 100;
            buttonStyle.fixedHeight = 26;

            GUILayout.BeginHorizontal(buttonsStyle);

            EditorGUI.BeginChangeCheck();
            GUILayout.Button("ADD", buttonStyle);

            if (EditorGUI.EndChangeCheck())
            {
                UiLayoutPreset itemNew = new UiLayoutPreset();

                itemNew.Container = layoutSettings.DefaultContainer;

                list.Add(itemNew);
                listItemSelected = itemNew;
            }

            if (listItemSelected != null)
            {
                EditorGUI.BeginChangeCheck();
                GUILayout.Button("REMOVE", buttonStyle);
                if (EditorGUI.EndChangeCheck())
                {
                    list.Remove(listItemSelected);
                    listItemSelected = null;
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            return listItemSelected;
        }

        private UiLayoutPreset RenderListItem(List<UiLayoutPreset> list, UiLayoutPreset item, int index, UiLayoutPreset listItemSelected, ItemType type)
        {
            GUIStyle style = new GUIStyle();
            style.padding.left = 15;

            GUILayout.BeginVertical(new GUIStyle(GUI.skin.GetStyle("HelpBox")));
            GUILayout.BeginVertical(style);

            // Foldout with item name
            listItemSelected = RenderListItemFoldOut(item, listItemSelected, type);

            // Render if is selected (foldouted)
            if (item == listItemSelected)
            {
                if (string.IsNullOrEmpty(item.PrefabPath))
                {
                    GUILayout.Label("Drag and drop prefab to setup this preset");
                }
                else
                {
                    GUILayout.Label(item.PrefabPath);
                }

                EditorGUILayout.Space();

                // Screen property
                RenderListItemPrefab(list, item, type);

                // Container property
                RenderListItemContainer(list, item, type);

                // Signals
                RenderListItemSignals(list, item, type);

                // Active by default
                RenderListItemActiveByDefault(list, item, type);
            }

            GUILayout.EndVertical();
            GUILayout.EndVertical();

            return listItemSelected;
        }

        // Render item element foldout
        private UiLayoutPreset RenderListItemFoldOut(UiLayoutPreset item, UiLayoutPreset listItemSelected, ItemType type)
        {
            bool foldout = item == listItemSelected;

            string name = "Undefined";

            if (string.IsNullOrEmpty(item.PrefabPath))
            {
                switch (type)
                {
                    case ItemType.Popup:
                        name = "Undefined popup preset";
                        break;

                    case ItemType.Screen:
                        name = "Undefined screen preset";
                        break;

                    case ItemType.Panel:
                        name = "Undefined panel preset";
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case ItemType.Popup:
                        name = GetItemPrefabName(item);
                        break;

                    case ItemType.Screen:
                    case ItemType.Panel:
                        name = GetItemPrefabName(item) + (item.ActiveByDefault ? " (active by default)" : string.Empty);
                        break;
                }
            }

            EditorGUI.BeginChangeCheck();

            foldout = EditorGUILayout.Foldout(foldout, name, true);

            if (EditorGUI.EndChangeCheck())
            {
                listItemSelected = foldout ? item : null;
            }

            return listItemSelected;
        }

        // Render item element prefab field
        private void RenderListItemPrefab(List<UiLayoutPreset> list, UiLayoutPreset item, ItemType type)
        {
            GameObject prefabOriginal = Resources.Load<GameObject>(item.PrefabPath);
            UiLayoutElement prefab = prefabOriginal != null ? prefabOriginal.GetComponent<UiLayoutElement>() : null;

            // Screen property
            EditorGUI.BeginChangeCheck();
            switch (type)
            {
                case ItemType.Popup:
                    prefab = (UiLayoutElementPopup)EditorGUILayout.ObjectField("Prefab", prefab, typeof(UiLayoutElementPopup), allowSceneObjects: false);
                    break;
                case ItemType.Panel:
                    prefab = (UiLayoutElementPanel)EditorGUILayout.ObjectField("Prefab", prefab, typeof(UiLayoutElementPanel), allowSceneObjects: false);
                    break;
                case ItemType.Screen:
                    prefab = (UiLayoutElementScreen)EditorGUILayout.ObjectField("Prefab", prefab, typeof(UiLayoutElementScreen), allowSceneObjects: false);
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (prefab != null)
                {
                    item.PrefabPath = AssetDatabase.GetAssetPath(prefab.gameObject);
                    item.PrefabPath = item.PrefabPath.Replace(".prefab", string.Empty);
                    item.PrefabPath = item.PrefabPath.Replace("Assets/Resources/", string.Empty);

                    switch (type)
                    {
                        case ItemType.Popup:
                        case ItemType.Panel:
                            item.SignalShow = "Ui." + GetItemPrefabName(item) + ".Show";
                            item.SignalHide = "Ui." + GetItemPrefabName(item) + ".Hide";
                            break;
                        case ItemType.Screen:
                            item.SignalShow = "Ui." + GetItemPrefabName(item) + ".Show";
                            item.SignalHide = "Ui.DynamicScreens.Hide";
                            break;
                    }
                }
                else
                {
                    item.PrefabPath = string.Empty;
                    item.SignalShow = string.Empty;
                    item.SignalHide = string.Empty;
                }
            }
        }

        // Render item element container field
        private void RenderListItemContainer(List<UiLayoutPreset> list, UiLayoutPreset item, ItemType type)
        {
            EditorGUI.BeginChangeCheck();
            item.Container = (RectTransform)EditorGUILayout.ObjectField("Container", item.Container, typeof(RectTransform), allowSceneObjects: true);
            if (EditorGUI.EndChangeCheck())
            {
            }
        }

        // Render item element signals fields
        private void RenderListItemSignals(List<UiLayoutPreset> list, UiLayoutPreset item, ItemType type)
        {
            item.SignalShow = EditorGUILayout.TextField("Signal Show", item.SignalShow);
            item.SignalHide = EditorGUILayout.TextField("Signal Hide", item.SignalHide);
        }

        // Render item element "active by default" togle
        private void RenderListItemActiveByDefault(List<UiLayoutPreset> list, UiLayoutPreset item, ItemType type)
        {
            if (type != ItemType.Popup)
            {
                // Active by default
                EditorGUI.BeginChangeCheck();
                item.ActiveByDefault = EditorGUILayout.Toggle("Active by default", item.ActiveByDefault);
                if (EditorGUI.EndChangeCheck())
                {
                    if (type == ItemType.Screen)
                    {
                        OnItemSetActiveByDefault(list, item, item.ActiveByDefault);
                    }
                }

                // Skip animation
                if (item.ActiveByDefault)
                {
                    item.ActiveByDefaultImmediately = EditorGUILayout.Toggle("Skip animation", item.ActiveByDefaultImmediately);
                }
            }
        }

        private void OnItemSetActiveByDefault(List<UiLayoutPreset> list, UiLayoutPreset item, bool value)
        {
            UiLayoutSettings layout = target as UiLayoutSettings;

            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] != item)
                {
                    list[i].ActiveByDefault = false;
                }
            }

            item.ActiveByDefault = value;
        }

        private string GetItemPrefabName(UiLayoutPreset preset)
        {
            if(preset == null || string.IsNullOrEmpty(preset.PrefabPath))
            {
                return string.Empty;
            }

            string[] levels = preset.PrefabPath.Split('/');

            return levels[levels.Length - 1];
        }
    }

}
#endif
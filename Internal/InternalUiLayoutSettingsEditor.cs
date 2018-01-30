#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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

        private ReorderableList _signalsReorderableList;

        private void OnEnable()
        {
            List<UiLayoutSettings.Signal> signals = InternalUtilities.GetSignals();

            _signalsReorderableList = new ReorderableList(signals, typeof(UiLayoutSettings.Signal), true, false, true, true);

            _signalsReorderableList.onReorderCallback = (ReorderableList target) =>
            {
                signals = target.list as List<UiLayoutSettings.Signal>;
            };

            _signalsReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 2;

                if (signals[index].Locked)
                {
                    EditorGUI.TextField
                    (
                        new Rect(rect.x, rect.y, rect.width - 50, EditorGUIUtility.singleLineHeight),
                        signals[index].Name
                    );
                }
                else
                {
                    signals[index].Name = EditorGUI.TextField
                    (
                        new Rect(rect.x, rect.y, rect.width - 50, EditorGUIUtility.singleLineHeight),
                        signals[index].Name
                    );
                }

                signals[index].Locked = EditorGUI.ToggleLeft
                (
                    new Rect(rect.x + rect.width - 45, rect.y, 50, EditorGUIUtility.singleLineHeight),
                    "Lock",
                    signals[index].Locked
                );

            };

            _signalsReorderableList.onAddCallback = (ReorderableList list) =>
            {
                InternalUtilities.AddSignal();
            };

            _signalsReorderableList.onRemoveCallback = (ReorderableList list) =>
            {
                if (signals[list.index].Locked)
                {
                    EditorUtility.DisplayDialog
                    (
                        "Warning!",
                        "You can not delete this signal because it is Locked.",
                        "Ok"
                    );

                    return;
                }

                if
                (
                    EditorUtility.DisplayDialog
                    (
                        "Warning!",
                        "Are you sure you want to delete signal \"" + signals[list.index].Name + "\"?", "Yes", "No"
                    )
                )
                {
                    ReorderableList.defaultBehaviours.DoRemoveButton(list);
                }
            };
        }

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

            EditorGUILayout.LabelField("Custom Signals");
            RenderSignals();

            EditorGUILayout.Space();
        }

        private void RenderSignals()
        {
            GUIStyle s = new GUIStyle();
            s.margin = new RectOffset(4, 4, 5, 5);

            GUILayout.BeginVertical(s);
            _signalsReorderableList.DoLayoutList();
            GUILayout.EndVertical();
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
            buttonsStyle.margin.left = -5;
            
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
                InternalUtilities.SetDirty(target);

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
                    InternalUtilities.SetDirty(target);

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
                InternalUtilities.SetDirty(target);

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
                InternalUtilities.SetDirty(target);

                if (prefab != null)
                {
                    Match match = Regex.Match
                    (
                        AssetDatabase.GetAssetPath(prefab.gameObject),
                        @"Resources/([A-Za-z0-9\-\/.]*).prefab",
                        RegexOptions.IgnoreCase
                    );

                    item.PrefabPath = match.Success ? match.Groups[1].Value : string.Empty;

                    UiLayoutSettings.Signal signalShow = InternalUtilities.AddSignal("Ui." + GetItemPrefabName(item) + ".Show");
                    UiLayoutSettings.Signal signalHide = InternalUtilities.AddSignal("Ui." + GetItemPrefabName(item) + ".Hide");

                    item.SignalsShow = item.SignalsShow.Push(signalShow.Id);
                    item.SignalsHide = item.SignalsHide.Push(signalHide.Id);
                }
                else
                {
                    item.PrefabPath = string.Empty;
                    //item.SignalShow = string.Empty;
                    //item.SignalHide = string.Empty;
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
                InternalUtilities.SetDirty(target);
            }
        }

        // Render item element signals fields
        private void RenderListItemSignals(List<UiLayoutPreset> list, UiLayoutPreset item, ItemType type)
        {
            InternalLayout.ButtonSignals
            (
                "Show on signals",
                "Select signals to show " + GetItemPrefabName(item),
                item.SignalsShow,
                (string[] result) =>
                {
                    item.SignalsShow = result;
                    Repaint();
                }
            );

            InternalLayout.ButtonSignals
            (
                "Hide on signals",
                "Select signals to hide " + GetItemPrefabName(item),
                item.SignalsHide,
                (string[] result) =>
                {
                    item.SignalsHide = result;
                    Repaint();
                }
            );
        }

        // Render item element "active by default" toggle
        private void RenderListItemActiveByDefault(List<UiLayoutPreset> list, UiLayoutPreset item, ItemType type)
        {
            if (type != ItemType.Popup)
            {
                // Active by default
                EditorGUI.BeginChangeCheck();
                item.ActiveByDefault = EditorGUILayout.Toggle("Active by default", item.ActiveByDefault);
                if (EditorGUI.EndChangeCheck())
                {
                    InternalUtilities.SetDirty(target);

                    if (type == ItemType.Screen)
                    {
                        OnItemSetActiveByDefault(list, item, item.ActiveByDefault);
                    }
                }

                // Skip animation
                if (item.ActiveByDefault)
                {
                    EditorGUI.BeginChangeCheck();
                    item.ActiveByDefaultImmediately = EditorGUILayout.Toggle("Skip animation", item.ActiveByDefaultImmediately);
                    if (EditorGUI.EndChangeCheck())
                    {
                        InternalUtilities.SetDirty(target);
                    }
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
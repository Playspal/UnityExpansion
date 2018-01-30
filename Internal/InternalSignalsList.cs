#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityExpansion.UI;

namespace UnityExpansionInternal
{
    public class InternalSignalsList : ScriptableWizard
    {
        public static InternalSignalsList Instance;

        public Action<string[]> OnChange;

        private Vector2 _scrollPosition;
        private ReorderableList _reorderableList;
        private List<string> _selectedSignals;
        private string _description;

        public void SetDescription(string value)
        {
            _description = value;
        }

        public void SetSelectedSignals(string[] input)
        {
            bool foundUnusedSignals = false;

            List<UiLayoutSettings.Signal> signals = InternalUtilities.GetSignals();

            _selectedSignals = new List<string>();

            if(input != null)
            {
                for(int i = 0; i < input.Length; i++)
                {
                    if(signals.Find(x => x.Id == input[i]) != null)
                    {
                        _selectedSignals.Add(input[i]);
                    }
                    else
                    {
                        foundUnusedSignals = true;
                    }
                }
            }

            if(foundUnusedSignals)
            {
                Callback();
            }
        }

        private void SignalAdd(string id)
        {
            if(!_selectedSignals.Contains(id))
            {
                _selectedSignals.Add(id);
            }

            Callback();
        }

        private void SignalRemove(string id)
        {
            if (_selectedSignals.Contains(id))
            {
                _selectedSignals.Remove(id);
            }

            Callback();
        }

        private void Callback()
        {
            string[] output = new string[_selectedSignals.Count];
            List<UiLayoutSettings.Signal> signals = InternalUtilities.GetSignals();

            int n = 0;

            for(int i = 0; i < signals.Count; i++)
            {
                if(_selectedSignals.Contains(signals[i].Id))
                {
                    output[n] = signals[i].Id;
                    n++;
                }
            }

            OnChange.InvokeIfNotNull(output);
        }

        private void OnEnable()
        {
            List<UiLayoutSettings.Signal> signals = InternalUtilities.GetSignals();

            _reorderableList = new ReorderableList(signals, typeof(UiLayoutSettings.Signal), true, false, true, true);

            _reorderableList.onReorderCallback = (ReorderableList target) =>
            {
                signals = target.list as List<UiLayoutSettings.Signal>;
            };

            _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 2;

                signals[index].Name = EditorGUI.TextField
                (
                    new Rect(rect.x + 20, rect.y, rect.width - 30, EditorGUIUtility.singleLineHeight),
                    signals[index].Name
                );

                bool isCheckedOld = _selectedSignals.Contains(signals[index].Id);
                bool isCheckedNew = EditorGUI.Toggle
                (
                    new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight),
                    isCheckedOld
                );

                if(isCheckedOld != isCheckedNew)
                {
                    if(isCheckedNew)
                    {
                        SignalAdd(signals[index].Id);
                    }
                    else
                    {
                        SignalRemove(signals[index].Id);
                    }
                }
            };

            _reorderableList.onAddCallback = (ReorderableList list) =>
            {
                InternalUtilities.AddSignal();
            };

            _reorderableList.onRemoveCallback = (ReorderableList list) =>
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

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

            EditorGUILayout.HelpBox("\n" + _description + "\n", MessageType.Info);

            GUIStyle s = new GUIStyle();
            s.margin = new RectOffset(4, 4, 5, 5);

            GUILayout.BeginVertical(s);
            _reorderableList.DoLayoutList();
            GUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }

        public void Update()
        {
            if(EditorWindow.focusedWindow != this)
            {
                Close();
            }
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        public static void ShowWindow(string description, string[] signals, Action<string[]> callback)
        {
            Instance = DisplayWizard<InternalSignalsList>("Select Signals");
            Instance.OnChange = callback;
            Instance.SetDescription(description);
            Instance.SetSelectedSignals(signals);
        }
    }
}
#endif
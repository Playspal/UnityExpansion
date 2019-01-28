#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityExpansion;
using UnityExpansion.UI;

namespace UnityExpansionInternal
{
    public class InternalSignalsList : ScriptableWizard
    {
        public static InternalSignalsList Instance;

        public Action<string[]> OnChange;

        private Vector2 _scrollPosition;

        private InternalSignalsEditor _signalsEditor;
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

            List<CommonPair<string, string>> signals = InternalSignalsFile.Load();

            _selectedSignals = new List<string>();

            if(input != null)
            {
                for(int i = 0; i < input.Length; i++)
                {
                    if(signals.Find(x => x.Value == input[i]) != null)
                    {
                        _selectedSignals.Add(input[i]);
                    }
                    else
                    {
                        foundUnusedSignals = true;
                    }
                }
            }

            _signalsEditor.SetSignalsSelected(_selectedSignals);

            if (foundUnusedSignals)
            {
                Callback();
            }
        }

        private void SignalAdd(string id)
        {
            Debug.LogError(id);

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
            List<CommonPair<string, string>> signals = InternalSignalsFile.Load();

            int n = 0;

            for(int i = 0; i < signals.Count; i++)
            {
                if(_selectedSignals.Contains(signals[i].Value))
                {
                    output[n] = signals[i].Value;
                    n++;
                }
            }

            _signalsEditor.SetSignalsSelected(_selectedSignals);

            OnChange.InvokeIfNotNull(output);
        }

        private void OnEnable()
        {
            _signalsEditor = new InternalSignalsEditor();
            _reorderableList = _signalsEditor.CreateEditorWithCheckboxes(SignalAdd, SignalRemove);
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
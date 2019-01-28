#if UNITY_EDITOR
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

using UnityExpansion;

namespace UnityExpansionInternal
{
    public class InternalSignalsEditor
    {
        private ReorderableList _reorderableList;

        private List<CommonPair<string, string>> _signals;
        private List<string> _signalsSelected = new List<string>();

        public ReorderableList CreateEditor()
        {
            Initialization();

            _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 2;

                string oldName = _signals[index].Key;
                string newName = EditorGUI.TextField
                (
                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    _signals[index].Key
                );

                if (oldName != newName)
                {
                    _signals[index].Key = Regex.Replace(newName, "[^0-9a-zA-Z]+", "");
                    InternalSignalsFile.Save(_signals);
                }
            };

            return _reorderableList;
        }

        public ReorderableList CreateEditorWithCheckboxes(Action<string> signalSelect, Action<string> signalUnselect)
        {
            Initialization();

            _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 2;

                string oldName = _signals[index].Key;
                string newName = EditorGUI.TextField
                (
                    new Rect(rect.x + 20, rect.y, rect.width - 20, EditorGUIUtility.singleLineHeight),
                    _signals[index].Key
                );

                if (oldName != newName)
                {
                    _signals[index].Key = Regex.Replace(newName, "[^0-9a-zA-Z]+", "");
                    InternalSignalsFile.Save(_signals);
                }

                
                bool isCheckedOld = _signalsSelected.Contains(_signals[index].Value);
                bool isCheckedNew = EditorGUI.Toggle
                (
                    new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight),
                    isCheckedOld
                );

                if (isCheckedOld != isCheckedNew)
                {
                    if (isCheckedNew)
                    {
                        signalSelect(_signals[index].Value);
                    }
                    else
                    {
                        signalUnselect(_signals[index].Value);
                    }
                }
            };

            return _reorderableList;
        }

        public void SetSignalsSelected(List<string> signalsSelected)
        {
            _signalsSelected = signalsSelected;
        }

        private void Initialization()
        {
            _signals = InternalSignalsFile.Load();

            _reorderableList = new ReorderableList(_signals, typeof(CommonPair<string, string>), true, false, true, true);

            _reorderableList.onAddCallback = OnAddCallback;
            _reorderableList.onRemoveCallback = OnRemoveCallBack;
            _reorderableList.onReorderCallback = OnReorderCallback;
            _reorderableList.onChangedCallback = OnChangedCallback;
        }

        private void OnReorderCallback(ReorderableList target)
        {
            _signals = target.list as List<CommonPair<string, string>>;
        }

        private void OnAddCallback(ReorderableList target)
        {
            _signals = InternalSignalsFile.Add();
            _reorderableList.list = _signals;
            _reorderableList.DoLayoutList();
        }

        private void OnRemoveCallBack(ReorderableList list)
        {
            if
            (
                EditorUtility.DisplayDialog
                (
                    "Warning!",
                    "Are you sure you want to delete signal \"" + _signals[list.index].Key + "\"?", "Yes", "No"
                )
            )
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
            }
        }

        private void OnChangedCallback(ReorderableList target)
        {
            InternalSignalsFile.Save(_signals);
        }
    }
}
#endif
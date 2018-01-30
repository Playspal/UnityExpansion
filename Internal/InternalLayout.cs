#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI;



namespace UnityExpansionInternal
{
    public static class InternalLayout
    {
        public static void ButtonSignals(string caption, string description, string[] signals, Action<string[]> callback, float offsetLeft = 0)
        {
            string title = "";

            if (signals == null || signals.Length == 0)
            {
                title = "None";
            }
            else if (signals.Length == 1)
            {
                title = InternalUtilities.GetSignalName(signals[0]);
            }
            else
            {
                title = InternalUtilities.GetSignalName(signals[0]) + ", ...";
            }

            var rect = EditorGUILayout.GetControlRect(false, 16f);
            rect = EditorGUI.PrefixLabel(rect, new GUIContent(caption));
            rect.x -= offsetLeft;
            rect.width += offsetLeft;
            EditorGUI.BeginChangeCheck();
            GUI.Button(rect, new GUIContent(title));
            if (EditorGUI.EndChangeCheck())
            {
                InternalSignalsList.ShowWindow
                (
                    description,
                    signals,
                    callback
                );
            }
        }
    }
}
#endif
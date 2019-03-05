#if UNITY_EDITOR
using System;

using UnityEditor;
using UnityEngine;

namespace UnityExpansionInternal
{
    public static class InternalLayout
    {
        public static void Button(string label, string caption, Action onclick, float offsetLeft = 0)
        {
            var rect = EditorGUILayout.GetControlRect(false, 16f);
            rect = EditorGUI.PrefixLabel(rect, new GUIContent(label));
            rect.x -= offsetLeft;
            rect.width += offsetLeft;

            EditorGUI.BeginChangeCheck();

            GUI.Button(rect, new GUIContent(caption));

            if (EditorGUI.EndChangeCheck())
            {
                onclick.InvokeIfNotNull();
            }
        }
    }
}
#endif
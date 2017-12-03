#if UNITY_EDITOR
using System;

using UnityEditor;
using UnityEngine;

namespace UnityExpansionInternal
{
    public static class InternalUiAnimationEditorGUI
    {
        public static void DrawTexture(float x, float y, float width, float height, Texture texture)
        {
            GUI.DrawTexture(new Rect(x, y, width, height), texture, ScaleMode.StretchToFill, true, 1f);
        }

        public static void Button(string caption, Action onclick = null)
        {
            EditorGUI.BeginChangeCheck();
            GUILayout.Button(caption);

            if (EditorGUI.EndChangeCheck())
            {
                onclick.InvokeIfNotNull();
            }
        }

        public static void Button(Texture icon, GUIStyle style, Action onclick = null)
        {
            EditorGUI.BeginChangeCheck();
            GUILayout.Button(icon, style);

            if (EditorGUI.EndChangeCheck())
            {
                onclick.InvokeIfNotNull();
            }
        }

        public static int Popup(int value, string[] items, GUIStyle style, Action<int> onchange = null)
        {
            EditorGUI.BeginChangeCheck();
            value = EditorGUILayout.Popup
            (
                value,
                items,
                style
            );

            if (EditorGUI.EndChangeCheck())
            {
                onchange.InvokeIfNotNull(value);
            }

            return value;
        }

        public static void Label(string text, int width)
        {
            Label(new GUIContent(text), width);
        }

        public static void Label(GUIContent content, int width)
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            labelStyle.fixedWidth = width;

            GUILayout.Label(content, labelStyle);
        }

        public static float InspectorFloatField(string label, float value)
        {
            GUILayout.BeginHorizontal();

            Label(label, 130);
            value = EditorGUILayout.FloatField(value);

            GUILayout.EndHorizontal();

            return value;
        }

        public static bool InspectorBooleanField(GUIContent content, bool value)
        {
            GUILayout.BeginHorizontal();

            Label(content, 130);
            value = EditorGUILayout.Toggle(value);

            GUILayout.EndHorizontal();

            return value;
        }

        public static Vector2 InspectorVector2Field(string label, Vector2 value)
        {
            GUILayout.BeginHorizontal();

            Label(label, 130);

            value.x = EditorGUILayout.FloatField(value.x);
            value.y = EditorGUILayout.FloatField(value.y);

            GUILayout.EndHorizontal();

            return value;
        }

        public static string InspectorTextField(string label, string value)
        {
            GUILayout.BeginHorizontal();

            Label(label, 130);
            value = EditorGUILayout.TextField(value);

            GUILayout.EndHorizontal();

            return value;
        }

        public static Enum InspectorEnumPopup(string label, Enum value, Action onchange = null)
        {
            GUILayout.BeginHorizontal();

            Label(label, 130);

            EditorGUI.BeginChangeCheck();
            value = EditorGUILayout.EnumPopup(value);
            if (EditorGUI.EndChangeCheck())
            {
                onchange.InvokeIfNotNull();
            }

            GUILayout.EndHorizontal();

            return value;
        }

        public static GameObject InspectorObjectField(string label, GameObject value)
        {
            GUILayout.BeginHorizontal();

            Label(label, 130);
            value = (GameObject)EditorGUILayout.ObjectField(value, typeof(GameObject), true);

            GUILayout.EndHorizontal();

            return value;
        }

        public static Color InspectorColorField(string label, Color value)
        {
            GUILayout.BeginHorizontal();

            Label(label, 130);
            value = EditorGUILayout.ColorField(value);

            GUILayout.EndHorizontal();

            return value;
        }
    }
}
#endif
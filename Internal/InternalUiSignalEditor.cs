#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

using UnityExpansion.UI;
using UnityExpansion.Utilities;

namespace UnityExpansionInternal
{
    [CustomEditor(typeof(UiSignal))]
    public class InternalUiSignalEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            UiSignal uiSignal = target as UiSignal;
            UiSignal.Type uiSignalType = (UiSignal.Type)serializedObject.FindProperty("_type").enumValueIndex;

            GUI.SetNextControlName("SignalType");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_type"));

            if (uiSignalType == UiSignal.Type.KeyPress || uiSignalType == UiSignal.Type.KeyRelease)
            {
                KeyCode keyCode = (KeyCode)UtilityReflection.GetMemberValue(uiSignal, "_keyCode");

                GUI.SetNextControlName("KeyCodeField");
                EditorGUILayout.TextField(new GUIContent("    Key"), keyCode.ToString());

                if (GUI.GetNameOfFocusedControl() == "KeyCodeField")
                {
                    if (Event.current.type == EventType.KeyUp)
                    {
                        UtilityReflection.SetMemberValue(uiSignal, "_keyCode", Event.current.keyCode);
                        GUI.FocusControl(null);
                    }
                }
            }

            InternalLayout.ButtonSignals
            (
                "Signals to dispatch",
                "Select signals to dispatch",
                UtilityReflection.GetMemberValue(uiSignal, "_signals") as string[],
                (string[] result) =>
                {
                    UtilityReflection.SetMemberValue(uiSignal, "_signals", result);
                    Repaint();
                }
            );

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
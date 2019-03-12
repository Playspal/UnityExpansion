using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI.Layout;

namespace UnityExpansionInternal
{
    [CustomEditor(typeof(UiLayoutButton), true)]
    public class InspectorUiLayoutButton : Editor
    {
        public override void OnInspectorGUI()
        {
            UiLayoutButton button = target as UiLayoutButton;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_isEnabled"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_transitionType"));

            if(serializedObject.FindProperty("_transitionType").enumValueIndex == 1)
            {
                //Debug.LogError("ASD")
            }

            //button._transitionType = (UiButton.TransitionType)EditorGUILayout.EnumPopup("Transition Type", button._transitionType);

            serializedObject.ApplyModifiedProperties();

            //button. = EditorGUILayout.Toggle(new GUIContent("Enabled", "Is button interactable"), button.IsEnabled);

            //button._transitionType = (UiButton.TransitionType)EditorGUILayout.EnumPopup("Transition Type", button._transitionType);
        }
    }
}
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI;

namespace UnityExpansionInternal
{
    [CustomEditor(typeof(UiButton))]
    public class InternalUiButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UiButton button = target as UiButton;

            button.IsEnabled = EditorGUILayout.Toggle(new GUIContent("Enabled", "Is button interactable"), button.IsEnabled);

            button._transitionType = (UiButton.TransitionType)EditorGUILayout.EnumPopup("Transition Type", button._transitionType);

            switch (button._transitionType)
            {
                case UiButton.TransitionType.None:
                    break;

                case UiButton.TransitionType.ObjectsSwap:
                    button._objectNormal = (GameObject)EditorGUILayout.ObjectField("    Normal", button._objectNormal, typeof(GameObject), allowSceneObjects: true);
                    button._objectHover = (GameObject)EditorGUILayout.ObjectField("    Hover", button._objectHover, typeof(GameObject), allowSceneObjects: true);
                    button._objectPressed = (GameObject)EditorGUILayout.ObjectField("    Pressed", button._objectPressed, typeof(GameObject), allowSceneObjects: true);
                    button._objectDisabled = (GameObject)EditorGUILayout.ObjectField("    Disabled", button._objectDisabled, typeof(GameObject), allowSceneObjects: true);

                    button._transitionTime = EditorGUILayout.FloatField(new GUIContent("    Transition Time", ""), button._transitionTime);

                    break;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_captions"), new GUIContent("Captions [?]", "Array of text fields that can be changed dynamically"), includeChildren: true);

            button._interactionDelay = EditorGUILayout.FloatField(new GUIContent("Interaction Delay [?]", "Time in seconds to prevent undesirable interactions repeat like double click"), button._interactionDelay);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
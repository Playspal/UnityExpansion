using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI.Layout;
using UnityExpansion.Utilities;

namespace UnityExpansionInternal
{
    [CustomEditor(typeof(UiLayoutButton), true)]
    public class InspectorUiLayoutButton : Editor
    {
        private const int TRANSITION_TYPE_NONE = 0;
        private const int TRANSITION_TYPE_OBJECTS_SWAP = 1;

        public override void OnInspectorGUI()
        {
            UiLayoutButton button = target as UiLayoutButton;

            DrawPropertyToggle(button, "_isEnabled", new GUIContent("Enabled", "Is button interactable"));
            DrawPropertyEnum(button, "_transitionType", new GUIContent("Transition type"), OnTransitionTypeChanged);

            UiLayoutButton.TransitionType transitionType = (UiLayoutButton.TransitionType)UtilityReflection.GetMemberValue(button, "_transitionType");

            switch(transitionType)
            {
                case UiLayoutButton.TransitionType.ObjectsSwap:

                    UiLayoutButtonTransitionObjectsSwap transition = UtilityReflection.GetMemberValue(button, "_transition") as UiLayoutButtonTransitionObjectsSwap;

                    if(transition != null)
                    {
                        DrawPropertyGameObject(transition, "_objectNormal", new GUIContent("  State normal"));
                        DrawPropertyGameObject(transition, "_objectHover", new GUIContent("  State hover"));
                        DrawPropertyGameObject(transition, "_objectPressed", new GUIContent("  State pressed"));
                        DrawPropertyGameObject(transition, "_objectDisabled", new GUIContent("  State disabled"));
                    }

                    break;
            }
        }

        private bool DrawPropertyToggle(object target, string name, GUIContent content, Action<bool> onChange = null)
        {
            bool valueBefore = (bool)UtilityReflection.GetMemberValue(target, name);
            bool valueAfter = EditorGUILayout.Toggle(content, valueBefore);

            if (valueAfter != valueBefore)
            {
                UtilityReflection.SetMemberValue(target, name, valueAfter);
                onChange.InvokeIfNotNull(valueAfter);
            }

            return valueAfter;
        }

        private Enum DrawPropertyEnum(object target, string name, GUIContent content, Action<Enum> onChange = null)
        {
            Enum valueBefore = (Enum)UtilityReflection.GetMemberValue(target, name);
            Enum valueAfter = EditorGUILayout.EnumPopup(content, valueBefore as Enum);

            if (!valueAfter.Equals(valueBefore))
            {
                UtilityReflection.SetMemberValue(target, name, valueAfter);
                onChange.InvokeIfNotNull(valueAfter);
            }

            return valueAfter;
        }

        private void DrawPropertyGameObject(object target, string name, GUIContent content)
        {
            GameObject valueBefore = (GameObject)UtilityReflection.GetMemberValue(target, name);
            GameObject valueAfter = (GameObject)EditorGUILayout.ObjectField(content, valueBefore, typeof(GameObject), allowSceneObjects: true);

            if (valueAfter != valueBefore)
            {
                UtilityReflection.SetMemberValue(target, name, valueAfter);

                Debug.LogError(UtilityReflection.GetMemberValue(target, name));
            }
        }
        
        private void OnTransitionTypeChanged(Enum value)
        {
            UiLayoutButton.TransitionType transitionType = (UiLayoutButton.TransitionType)value;

            switch(transitionType)
            {
                case UiLayoutButton.TransitionType.None:
                    Debug.LogError("Removed");
                    UtilityReflection.SetMemberValue(target, "_transition", null);
                    break;


                case UiLayoutButton.TransitionType.ObjectsSwap:
                    Debug.LogError("Created");
                    UtilityReflection.SetMemberValue(target, "_transition", new UiLayoutButtonTransitionObjectsSwap());
                    break;
            }

            /*
            if (transitionType == TRANSITION_TYPE_OBJECTS_SWAP)
            {
                UiLayoutButton button = target as UiLayoutButton;
                SetTransition(button.gameObject.AddComponent<UiLayoutButtonTransitionObjectsSwap>());
            }
            else
            {
                UiLayoutButton button = target as UiLayoutButton;
                UiLayoutButtonTransition transition = GetTransition();
                
                if(transition != null)
                {
                    DestroyImmediate(transition);
                }

                SetTransition(null);
            }
            */
        }
    }
}
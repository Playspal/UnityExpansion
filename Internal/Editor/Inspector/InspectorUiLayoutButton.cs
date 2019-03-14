using System;

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

                    transition.hideFlags = HideFlags.HideInInspector;

                    if (transition != null)
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
            }
        }
        
        private void OnTransitionTypeChanged(Enum value)
        {
            UiLayoutButton.TransitionType transitionType = (UiLayoutButton.TransitionType)value;
            UiLayoutButtonTransition transition;

            switch (transitionType)
            {
                case UiLayoutButton.TransitionType.None:
                    transition = UtilityReflection.GetMemberValue(target, "_transition") as UiLayoutButtonTransition;

                    if(transition != null)
                    {
                        DestroyImmediate(transition);
                    }

                    UtilityReflection.SetMemberValue(target, "_transition", null);
                    break;


                case UiLayoutButton.TransitionType.ObjectsSwap:
                    UiLayoutButton button = target as UiLayoutButton;
                    transition = button.gameObject.AddComponent<UiLayoutButtonTransitionObjectsSwap>();
                    transition.hideFlags = HideFlags.HideInInspector;

                    UtilityReflection.SetMemberValue(target, "_transition", transition);
                    break;
            }
        }
    }
}
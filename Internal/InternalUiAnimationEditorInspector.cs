#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityExpansion.Tweens;
using UnityExpansion.UI.Animation;
using UnityExpansion.Utilities;

namespace UnityExpansionInternal
{
    public class InternalUiAnimationEditorInspector
    {
        public static float Width = 300;

        private static bool _expandPlayOnBlock = true;

        public static void OnEvent()
        {

        }

        public static void Render(float positionX, float positionY, float width, float height)
        {
            InternalUiAnimationEditorGUI.DrawTexture(positionX + width - 1, positionY, 1, height, InternalUiAnimationEditorTextures.ColorBlack20);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            labelStyle.fixedWidth = 130;

            GUILayout.BeginArea(new Rect(positionX, positionY, width - 2, height), new GUIStyle());

            GUILayout.Space(5);
            GUILayout.Label("Animation Clip Settings");
            GUILayout.BeginVertical(new GUIStyle(GUI.skin.GetStyle("HelpBox")));
            GUILayout.Space(2);
            RenderAnimationClip(InternalUiAnimationEditorSelection.TargetAnimationClip);
            GUILayout.Space(2);
            GUILayout.EndVertical();

            if (InternalUiAnimationEditorSelection.CanvasItemToEdit != null)
            {
                GUILayout.Space(5);
                GUILayout.Label("Segment Settings");
                GUILayout.BeginVertical(new GUIStyle(GUI.skin.GetStyle("HelpBox")));
                GUILayout.Space(2);
                RenderAnimationClipSegment(InternalUiAnimationEditorSelection.CanvasItemToEdit.Target);
                GUILayout.Space(2);
                GUILayout.EndVertical();
            }

            GUILayout.EndArea();
        }

        private static void RenderAnimationClip(UiAnimationClip clip)
        {
            clip.Name = InternalUiAnimationEditorGUI.InspectorTextField
            (
                "Name",
                clip.Name
            );

            clip.Loop = InternalUiAnimationEditorGUI.InspectorBooleanField
            (
                new GUIContent("Loop animation"),
                clip.Loop
            );

            List<string> triggers = new List<string>();

            if (clip.PlayOnAwake)
            {
                triggers.Add("Awake");
            }

            if (clip.PlayOnLayoutElementShow)
            {
                triggers.Add("Element Show");
            }

            if (clip.PlayOnLayoutElementHide)
            {
                triggers.Add("Element Hide");
            }

            if (clip.PlayOnSignals.Length > 0)
            {
                triggers.Add("Signals");
            }

            string playon = "";

            if (triggers.Count > 1)
            {
                playon += " " + triggers.Count + " triggers";
            }
            else if (triggers.Count > 0)
            {
                playon += " " + triggers[0];
            }
            else
            {
                playon = " ...";
            }

            _expandPlayOnBlock = EditorGUILayout.Foldout(_expandPlayOnBlock, "Play on" + playon, true);

            if (_expandPlayOnBlock)
            {
                clip.PlayOnAwake = InternalUiAnimationEditorGUI.InspectorBooleanField
                (
                    new GUIContent("   Awake"),
                    clip.PlayOnAwake
                );

                clip.PlayOnLayoutElementShow = InternalUiAnimationEditorGUI.InspectorBooleanField
                (
                    new GUIContent("   Element show"),
                    clip.PlayOnLayoutElementShow
                );

                clip.PlayOnLayoutElementHide = InternalUiAnimationEditorGUI.InspectorBooleanField
                (
                    new GUIContent("   Element hide"),
                    clip.PlayOnLayoutElementHide
                );

                InternalLayout.ButtonSignals
                (
                    "   Signals",
                    "Select signals to play animation clip",
                    clip.PlayOnSignals,
                    (string[] result) =>
                    {
                        clip.PlayOnSignals = result;
                    },
                    15
                );
            }
        }

        private static void RenderAnimationClipSegment(UiAnimationClipSegment segment)
        {
            if (segment.GameObject == null)
            {
                EditorGUILayout.HelpBox("Game object is missed! Setup another game object or delete this segment.", MessageType.Error);
            }

            segment.GameObject = InternalUiAnimationEditorGUI.InspectorObjectField
            (
                "Game Object",
                segment.GameObject
            );

            if (segment.GameObject != null)
            {

                segment.ItemType = (UiAnimationClipSegmentType)InternalUiAnimationEditorGUI.InspectorEnumPopup
            (
                "Type",
                segment.ItemType,
                () =>
                {
                    RectTransform rectTransform = segment.GameObject.GetComponent<RectTransform>();

                    if (rectTransform != null)
                    {
                        segment.PositionFrom = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
                        segment.PositionTo = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

                        segment.RotationFrom = rectTransform.localRotation.eulerAngles.z;
                        segment.RotationTo = rectTransform.localRotation.eulerAngles.z;

                        segment.ScaleFrom = new Vector2(rectTransform.localScale.x, rectTransform.localScale.y);
                        segment.ScaleTo = new Vector2(rectTransform.localScale.x, rectTransform.localScale.y);
                    }
                }
            );

                segment.EasingType = (EasingType)InternalUiAnimationEditorGUI.InspectorEnumPopup
                (
                    "Easing type",
                    segment.EasingType
                );

                switch (segment.ItemType)
                {
                    case UiAnimationClipSegmentType.Alpha:
                        segment.AlphaFrom = InternalUiAnimationEditorGUI.InspectorFloatField("Alpha From", segment.AlphaFrom);
                        segment.AlphaTo = InternalUiAnimationEditorGUI.InspectorFloatField("Alpha To", segment.AlphaTo);
                        break;

                    case UiAnimationClipSegmentType.Color:
                        segment.ColorFrom = InternalUiAnimationEditorGUI.InspectorColorField("Color From", segment.ColorFrom);
                        segment.ColorTo = InternalUiAnimationEditorGUI.InspectorColorField("Color To", segment.ColorTo);
                        break;

                    case UiAnimationClipSegmentType.Position:
                        segment.PositionFrom = InternalUiAnimationEditorGUI.InspectorVector2Field("Position From XY", segment.PositionFrom);
                        segment.PositionTo = InternalUiAnimationEditorGUI.InspectorVector2Field("Position To XY", segment.PositionTo);
                        break;

                    case UiAnimationClipSegmentType.Rotation:
                        segment.RotationFrom = InternalUiAnimationEditorGUI.InspectorFloatField("Rotation From", segment.RotationFrom);
                        segment.RotationTo = InternalUiAnimationEditorGUI.InspectorFloatField("Rotation To", segment.RotationTo);
                        break;

                    case UiAnimationClipSegmentType.Scale:
                        segment.ScaleFrom = InternalUiAnimationEditorGUI.InspectorVector2Field("Scale From XY", segment.ScaleFrom);
                        segment.ScaleTo = InternalUiAnimationEditorGUI.InspectorVector2Field("Scale To XY", segment.ScaleTo);
                        break;
                }

                segment.Predefined = InternalUiAnimationEditorGUI.InspectorBooleanField
                (
                    new GUIContent("Predefined", "Use From value as predefined value for " + segment.GameObject.name),
                    segment.Predefined
                );

                /*
                UtilityReflection.SetMemberValue
                (
                    segment, "Predefined",
                    segment.Predefined = InternalUiAnimationEditorGUI.InspectorBooleanField
                    (
                        new GUIContent("Predefined", "Use From value as predefined value for " + segment.GameObject.name),
                        (bool)UtilityReflection.GetMemberValue(segment, "Predefined")
                    )
                );
                */
            }

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.GetStyle("Button"));
            buttonStyle.margin.left = 138;
            buttonStyle.margin.top = 5;

            EditorGUI.BeginChangeCheck();
            GUILayout.Button("Delete segment", buttonStyle);
            if (EditorGUI.EndChangeCheck())
            {
                InternalUiAnimationEditorSelection.TargetAnimationClip.Items.Remove(segment);
                InternalUiAnimationEditorSelection.TargetAnimationClip.Items.Sort((a, b) => (a.Delay.CompareTo(b.Delay)));
                InternalUiAnimationEditorSelection.SetCanvasItemToEdit(null);
            }
        }
    }
}
#endif
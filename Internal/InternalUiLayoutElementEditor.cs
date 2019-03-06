#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using UnityExpansion.UI.Animation;
using UnityExpansion.UI.Layout;

namespace UnityExpansionInternal
{
    [CustomEditor(typeof(UiLayoutElement), true)]
    public class InternalUiLayoutElementEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(5);

            UiLayoutElement layoutElement = target as UiLayoutElement;
            UiAnimation animation = layoutElement.GetComponent<UiAnimation>();

            if (animation != null)
            {
                int indexShow = 0;
                int indexHide = 0;

                string[] animationsNames = new string[animation.AnimationClips.Count + 1];
                string[] animationsIDs = new string[animation.AnimationClips.Count + 1];

                animationsNames[0] = "None";

                for (int i = 0; i < animation.AnimationClips.Count; i++)
                {
                    animationsNames[i + 1] = animation.AnimationClips[i].Name;
                    animationsIDs[i + 1] = animation.AnimationClips[i].ID.ToString();
                }

                for (int i = 0; i < animationsNames.Length; i++)
                {
                    UiAnimationClip clip = animation.GetAnimationClip(animationsNames[i]);
                    UiAnimationClip clipShow = animation.GetAnimationClip(layoutElement.AnimationShowID);
                    UiAnimationClip clipHide = animation.GetAnimationClip(layoutElement.AnimationHideID);

                    if (clipShow == clip)
                    {
                        indexShow = i;
                    }

                    if (clipHide == clip)
                    {
                        indexHide = i;
                    }
                }

                EditorGUI.BeginChangeCheck();
                indexShow = EditorGUILayout.Popup("Animation Show", indexShow, animationsNames);
                if (EditorGUI.EndChangeCheck())
                {
                    layoutElement.AnimationShowID = indexShow > 0 ? animationsIDs[indexShow] : null;
                }

                EditorGUI.BeginChangeCheck();
                indexHide = EditorGUILayout.Popup("Animation Hide", indexHide, animationsNames);
                if (EditorGUI.EndChangeCheck())
                {
                    layoutElement.AnimationHideID = indexHide > 0 ? animationsIDs[indexHide] : null;
                }
            }
            else
            {
                layoutElement.AnimationShowID = null;
                layoutElement.AnimationHideID = null;
                EditorGUILayout.HelpBox("\nAdd UiTweener component to create Show and Hide animations.\n", MessageType.Info);
            }
        }
    }
}
#endif
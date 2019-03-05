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

                string[] animations = new string[animation.AnimationClips.Count + 1];

                animations[0] = "None";

                for (int i = 0; i < animation.AnimationClips.Count; i++)
                {
                    animations[i + 1] = animation.AnimationClips[i].Name;
                }

                for (int i = 0; i < animations.Length; i++)
                {
                    if (layoutElement.AnimationShow != null && layoutElement.AnimationShow == animations[i])
                    {
                        indexShow = i;
                    }

                    if (layoutElement.AnimationHide != null && layoutElement.AnimationHide == animations[i])
                    {
                        indexHide = i;
                    }
                }

                EditorGUI.BeginChangeCheck();
                indexShow = EditorGUILayout.Popup("Animation Show", indexShow, animations);
                if (EditorGUI.EndChangeCheck())
                {
                    layoutElement.AnimationShow = indexShow > 0 ? animations[indexShow] : null;
                }

                EditorGUI.BeginChangeCheck();
                indexHide = EditorGUILayout.Popup("Animation Hide", indexHide, animations);
                if (EditorGUI.EndChangeCheck())
                {
                    layoutElement.AnimationHide = indexHide > 0 ? animations[indexHide] : null;
                }
            }
            else
            {
                layoutElement.AnimationShow = null;
                layoutElement.AnimationHide = null;
                EditorGUILayout.HelpBox("\nAdd UiTweener component to create Show and Hide animations.\n", MessageType.Info);
            }
        }
    }
}
#endif
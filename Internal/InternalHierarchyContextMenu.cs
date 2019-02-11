#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using UnityExpansion;
using UnityExpansion.UI;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal
{
    public static class InternalHierarchyContextMenu
    {
        /// <summary>
        /// Creates new expansion main object.
        /// </summary>
        [MenuItem("GameObject/UnityExpansion/Main Object", false, 0)]
        public static void CreateExpansionMainObject()
        {
            Expansion[] exists = GameObject.FindObjectsOfType<Expansion>();

            if(exists.Length > 0)
            {
                EditorUtility.DisplayDialog("Error", "UnityExpansion main object already exists in screne.", "OK");
                return;
            }

            GameObject gameObject = new GameObject("UnityExpansion");
            Expansion expansion = gameObject.AddComponent<Expansion>();

            gameObject.transform.position = Vector3.zero;
            gameObject.transform.SetParent(Selection.activeTransform, false);

            Selection.activeObject = gameObject;
        }

        /// <summary>
        /// Creates new layout screen.
        /// </summary>
        [MenuItem("GameObject/UnityExpansion/Layout", false, 0)]
        public static void CreateExpansionLayout()
        {
            GameObject gameObject = new GameObject("Layout");
            RectTransform rectTransform = CreateRectTransform(gameObject, Selection.activeTransform);
            UiLayout element = gameObject.AddComponent<UiLayout>();

            Selection.activeObject = gameObject;
        }

        /// <summary>
        /// Creates new layout element.
        /// </summary>
        [MenuItem("GameObject/UnityExpansion/LayoutElement", false, 0)]
        public static void CreateExpansionLayoutElement()
        {
            GameObject gameObject = new GameObject("LayoutElement");
            RectTransform rectTransform = CreateRectTransform(gameObject, Selection.activeTransform);
            UiLayoutElement element = gameObject.AddComponent<UiLayoutElement>();

            UiAnimationClip animationShow = new UiAnimationClip
            {
                Name = "Show animation",
                PlayOnLayoutElementShow = true
            };

            UiAnimationClip animationHide = new UiAnimationClip
            {
                Name = "Hide animation",
                PlayOnLayoutElementHide = true
            };

            UiAnimation animation = gameObject.AddComponent<UiAnimation>();
            animation.AnimationClips = new System.Collections.Generic.List<UiAnimationClip>
            {
                animationShow,
                animationHide
            };

            element.AnimationShow = animationShow.Name;
            element.AnimationHide = animationHide.Name;

            Selection.activeObject = gameObject;
        }
        
        /// <summary>
        /// Creates new layer inside of Canvas.
        /// </summary>
        [MenuItem("GameObject/UI/Empty Object", false, 0)]
        public static void CreateCanvasLayer()
        {
            GameObject gameObject = new GameObject("Empty");
            RectTransform rectTransform = CreateRectTransform(gameObject, Selection.activeTransform);

            Selection.activeObject = gameObject;
        }

        private static RectTransform CreateRectTransform(GameObject gameObject, Transform parent)
        {
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);
            rectTransform.SetParent(parent, false);

            return rectTransform;
        }
    }
}
#endif
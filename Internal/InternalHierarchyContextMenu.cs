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
        [MenuItem("GameObject/UnityExpansion/Layout Screen", false, 0)]
        public static void CreateExpansionLayoutScreen()
        {
            GameObject gameObject = new GameObject("LayoutElementScreen");
            RectTransform rectTransform = CreateRectTransform(gameObject, Selection.activeTransform);
            UiLayoutElementScreen element = gameObject.AddComponent<UiLayoutElementScreen>();

            UiAnimationClip animationShow = new UiAnimationClip
            {
                Name = "Screen Show Animation",
                PlayOnLayoutElementShow = true
            };

            UiAnimationClip animationHide = new UiAnimationClip
            {
                Name = "Screen Hide Animation",
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
        /// Creates new layout panel.
        /// </summary>
        [MenuItem("GameObject/UnityExpansion/Layout Panel", false, 0)]
        public static void CreateExpansionLayoutPanel()
        {
            GameObject gameObject = new GameObject("LayoutElementPanel");
            RectTransform rectTransform = CreateRectTransform(gameObject, Selection.activeTransform);
            UiLayoutElementPanel element = gameObject.AddComponent<UiLayoutElementPanel>();

            Selection.activeObject = gameObject;
        }

        /// <summary>
        /// Creates new layout popup.
        /// </summary>
        [MenuItem("GameObject/UnityExpansion/Layout Popup", false, 0)]
        public static void CreateExpansionLayoutPopup()
        {
            GameObject gameObject = new GameObject("LayoutElementPopup");
            RectTransform rectTransform = CreateRectTransform(gameObject, Selection.activeTransform);
            UiLayoutElementPopup element = gameObject.AddComponent<UiLayoutElementPopup>();

            GameObject background = new GameObject("Background");
            RectTransform backgroundRectTransform = CreateRectTransform(background, rectTransform);
            Image backgroundImage = background.AddComponent<Image>();

            backgroundImage.color = new Color(0, 0, 0, 0.5f);

            GameObject window = new GameObject("Window");
            RectTransform windowRectTransform = window.AddComponent<RectTransform>();
            Image windowImage = window.AddComponent<Image>();

            windowRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 360);
            windowRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 180);
            windowRectTransform.SetParent(rectTransform, false);

            windowImage.color = new Color(.9f, .9f, .9f, 1f);

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
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal
{
    public class InternalUiAnimationEditorSelection
    {
        /// <summary>
        /// Selected GameObject in scene hierarchy.
        /// </summary>
        public static GameObject TargetGameObject = null;

        /// <summary>
        /// Selected UiAnimation component.
        /// </summary>
        public static UiAnimation TargetAnimation
        {
            get
            {
                if (TargetGameObject == null)
                {
                    return null;
                }

                return TargetGameObject.GetComponent<UiAnimation>();
            }
        }

        /// <summary>
        /// Selected UiAnimationClip.
        /// </summary>
        public static UiAnimationClip TargetAnimationClip
        {
            get
            {
                if (TargetAnimation == null)
                {
                    return null;
                }

                if (TargetAnimation.AnimationClips.Count == 0)
                {
                    return null;
                }

                if (TargetAnimation.AnimationClips.Count <= TargetAnimationClipIndex)
                {
                    return null;
                }

                return TargetAnimation.AnimationClips[TargetAnimationClipIndex];
            }
        }

        /// <summary>
        /// Index of selected UiAnimationClip in UiAnimation's list.
        /// </summary>
        public static int TargetAnimationClipIndex { get; private set; }

        /// <summary>
        /// CanvasItem selected to edit in inspector.
        /// </summary>
        public static InternalUiAnimationEditorCanvasItem CanvasItemToEdit { get; private set; }

        /// <summary>
        /// CanvasItem selected to resize.
        /// </summary>
        public static InternalUiAnimationEditorCanvasItem CanvasItemToResize { get; private set; }

        /// <summary>
        /// CanvasItem selected to drag.
        /// </summary>
        public static InternalUiAnimationEditorCanvasItem CanvasItemToDrag { get; private set; }

        private static bool _queueCanvasItemToEditChanged = false;
        private static bool _queueCanvasItemToResizeChanged = false;
        private static bool _queueCanvasItemToDragChanged = false;

        private static InternalUiAnimationEditorCanvasItem _queueCanvasItemToEdit;
        private static InternalUiAnimationEditorCanvasItem _queueCanvasItemToResize;
        private static InternalUiAnimationEditorCanvasItem _queueCanvasItemToDrag;

        public static void SetTargetAnimationClipIndex(int value)
        {
            TargetAnimationClipIndex = value;

            CanvasItemToEdit = null;

            _queueCanvasItemToEdit = null;
            _queueCanvasItemToEditChanged = false;
        }

        public static void SetCanvasItemToEdit(InternalUiAnimationEditorCanvasItem item)
        {
            _queueCanvasItemToEdit = item;
            _queueCanvasItemToEditChanged = true;
        }

        public static void SetCanvasItemToResize(InternalUiAnimationEditorCanvasItem item)
        {
            _queueCanvasItemToResize = item;
            _queueCanvasItemToResizeChanged = true;
        }

        public static void SetCanvasItemToDrag(InternalUiAnimationEditorCanvasItem item)
        {
            _queueCanvasItemToDrag = item;
            _queueCanvasItemToDragChanged = true;
        }

        /// <summary>
        /// Updates selection.
        /// </summary>
        /// <returns>True if selection is changed.</returns>
        public static bool Update()
        {
            if (_queueCanvasItemToEditChanged)
            {
                CanvasItemToEdit = _queueCanvasItemToEdit;
                _queueCanvasItemToEdit = null;
                _queueCanvasItemToEditChanged = false;
            }

            if (_queueCanvasItemToResizeChanged)
            {
                CanvasItemToResize = _queueCanvasItemToResize;
                _queueCanvasItemToResize = null;
                _queueCanvasItemToResizeChanged = false;
            }

            if (_queueCanvasItemToDragChanged)
            {
                CanvasItemToDrag = _queueCanvasItemToDrag;
                _queueCanvasItemToDrag = null;
                _queueCanvasItemToDragChanged = false;
            }

            if (Selection.activeGameObject != TargetGameObject)
            {
                TargetGameObject = Selection.activeGameObject;
                TargetAnimationClipIndex = 0;

                CanvasItemToEdit = null;
                CanvasItemToResize = null;
                CanvasItemToDrag = null;

                return true;
            }

            return false;
        }

        public static string[] GetAnimationClipsNames()
        {
            if (TargetAnimation == null || TargetAnimation.AnimationClips.Count == 0)
            {
                return new string[0];
            }

            string[] output = new string[TargetAnimation.AnimationClips.Count];
            string spaces = "";

            for (int i = 0; i < TargetAnimation.AnimationClips.Count; i++)
            {
                output[i] = TargetAnimation.AnimationClips[i].Name + spaces;
                spaces += " ";
            }

            return output;
        }
    }
}
#endif
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityExpansion.Tweens;

namespace UnityExpansion.UI.Animation
{
    /// <summary>
    /// Used as part of UiAnimationClip.
    /// Provides object parameters interpolation by predefined rules.
    /// </summary>
    [Serializable]
    public class UiAnimationClipSegment
    {
        /// <summary>
        /// Segment type.
        /// </summary>
        public UiAnimationClipSegmentType ItemType = UiAnimationClipSegmentType.Alpha;

        /// <summary>
        /// Easing type.
        /// </summary>
        public EasingType EasingType = EasingType.Linear;

        /// <summary>
        /// Target game object.
        /// </summary>
        public GameObject GameObject;

        /// <summary>
        /// Layer number to place in animation editor.
        /// </summary>
        public float Layer = 0;

        /// <summary>
        /// Activation delay (offset) in seconds.
        /// </summary>
        public float Delay = 0;

        /// <summary>
        /// Duration in seconds.
        /// </summary>
        public float Duration = 0.5f;

        /// <summary>
        /// Use From value as predefined value on animation start.
        /// </summary>
        public bool Predefined = false;

        public float AlphaFrom = 0;
        public float AlphaTo = 0;
        public Color ColorFrom = Color.white;
        public Color ColorTo = Color.black;
        public Vector2 PositionFrom = Vector2.zero;
        public Vector2 PositionTo = Vector2.zero;
        public float RotationFrom = 0;
        public float RotationTo = 0;
        public Vector2 ScaleFrom = Vector2.zero;
        public Vector2 ScaleTo = Vector2.zero;

        /// <summary>
        /// Interpolates and sets value to target game object using specified position of animation clip.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        public void Goto(float time)
        {
            if (GameObject == null)
            {
                return;
            }

            if (time < Delay && !Predefined)
            {
                return;
            }

            float timeNormalized = (time - Delay) / Duration;
            timeNormalized = Mathf.Clamp(timeNormalized, 0, 1);

            switch (ItemType)
            {
                case UiAnimationClipSegmentType.Alpha:
                    SetAlpha(timeNormalized);
                    break;

                case UiAnimationClipSegmentType.Color:
                    SetColor(timeNormalized);
                    break;

                case UiAnimationClipSegmentType.Position:
                    SetPosition(timeNormalized);
                    break;

                case UiAnimationClipSegmentType.Scale:
                    SetScale(timeNormalized);
                    break;

                case UiAnimationClipSegmentType.Rotation:
                    SetRotation(timeNormalized);
                    break;
            }
        }

        // Interpolate alpha by normalized time.
        private void SetAlpha(float time)
        {
            GameObject.SetAlpha(Easing.Interpolate(AlphaFrom, AlphaTo, time, EasingType));
        }

        // Interpolate color by normalized time.
        private void SetColor(float time)
        {
            Graphic graphic = GameObject.GetComponent<Graphic>();

            if (graphic != null)
            {
                graphic.color = Easing.Interpolate(ColorFrom, ColorTo, time, EasingType);
            }
        }

        // Interpolate position by normalized time.
        private void SetPosition(float time)
        {
            RectTransform rectTransform = GameObject.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Easing.Interpolate(PositionFrom, PositionTo, time, EasingType);
            }
        }

        // Interpolate scale by normalized time.
        private void SetScale(float time)
        {
            RectTransform rectTransform = GameObject.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.localScale = Easing.Interpolate(ScaleFrom, ScaleTo, time, EasingType);
            }
        }

        // Interpolate rotation by normalized time.
        private void SetRotation(float time)
        {
            RectTransform rectTransform = GameObject.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                Vector3 localRotation = rectTransform.localRotation.eulerAngles;

                localRotation.z = Easing.Interpolate(RotationFrom, RotationTo, time, EasingType);

                rectTransform.localRotation = Quaternion.Euler(localRotation);
            }
        }
    }
}
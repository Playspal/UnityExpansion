using System;

namespace UnityExpansion.UI.Animation
{
    /// <summary>
    /// Presentes type of animation segment.
    /// </summary>
    [Serializable]
    public enum UiAnimationClipSegmentType
    {
        /// <summary>
        /// Interpolation of GameObject's alpha.
        /// </summary>
        Alpha,

        /// <summary>
        /// Interpolation of Graphic's color (text, image and raw image).
        /// </summary>
        Color,

        /// <summary>
        /// Interpolation of RectTransform's anchored position.
        /// </summary>
        Position,

        /// <summary>
        /// Interpolation of RectTransform's local rotation z.
        /// </summary>
        Rotation,

        /// <summary>
        /// Interpolation of RectTransform's local scale x and y.
        /// </summary>
        Scale,

        /// <summary>
        /// Interpolation of RectTransform's rect.width and rect.height.
        /// </summary>
        Size
    }
}
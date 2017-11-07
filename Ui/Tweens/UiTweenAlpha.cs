using UnityEngine;
using UnityExpansion.Tweens;

namespace UnityExpansion.UI.Tweens
{
    /// <summary>
    /// Provides linear change of Ui object's alpha by specified time.
    /// </summary>
    /// <seealso cref="UnityExpansion.Tweens.TweenLineal" />
    public class UiTweenAlpha : TweenLineal
    {
        // Canvas group of target object
        private CanvasGroup _canvasGroup;

        /// <summary>
        /// Initializes a new instance of the UiTweenAlpha.
        /// </summary>
        /// <param name="target">Target UiObject</param>
        /// <param name="alphaFrom">Start alpha value</param>
        /// <param name="alphaTo">Target alpha value</param>
        /// <param name="time">Tween time in seconds</param>
        /// <param name="type">Tween type</param>
        public UiTweenAlpha(UiObject target, float alphaFrom, float alphaTo, float time, TweenType type)
            : base(alphaFrom, alphaTo, time, type)
        {
            Setup(target.Graphics);
        }

        /// <summary>
        /// Initializes a new instance of the UiTweenAlpha.
        /// </summary>
        /// <param name="target">Target GameObject</param>
        /// <param name="alphaFrom">Start alpha value</param>
        /// <param name="alphaTo">Target alpha value</param>
        /// <param name="time">Tween time in seconds</param>
        /// <param name="type">Tween type</param>
        public UiTweenAlpha(GameObject target, float alphaFrom, float alphaTo, float time, TweenType type)
            : base(alphaFrom, alphaTo, time, type)
        {
            Setup(target);
        }

        // Setups tween
        private void Setup(GameObject target)
        {
            _canvasGroup = target.GetOrAddComponent<CanvasGroup>();
        }

        // Tween iteration
        internal override void Update(float value)
        {
            _canvasGroup.alpha = value;
        }
    }
}
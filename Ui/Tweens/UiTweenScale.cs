using UnityEngine;
using UnityExpansion.Tweens;

namespace UnityExpansion.UI.Tweens
{
    /// <summary>
    /// Provides linear change of Ui object's local scale by specified time.
    /// </summary>
    /// <seealso cref="UnityExpansion.Tweens.TweenLineal" />
    public class UiTweenScale : TweenLineal
    {
        private RectTransform _rectTransform;

        private Vector2 _scaleFrom;
        private Vector2 _scaleTo;

        /// <summary>
        /// Initializes a new instance of the UiTweenScale.
        /// </summary>
        /// <param name="target">Target UiObject</param>
        /// <param name="scaleFrom">Start scale</param>
        /// <param name="scaleTo">Target scale</param>
        /// <param name="time">Tween time in seconds</param>
        /// <param name="type">Tween type</param>
        public UiTweenScale(UiObject target, Vector2 scaleFrom, Vector2 scaleTo, float time, TweenType type)
            : base(0, 1, time, type)
        {
            Setup(target.GraphicsTransform, scaleFrom, scaleTo);
        }

        /// <summary>
        /// Initializes a new instance of the UiTweenScale.
        /// </summary>
        /// <param name="target">Target UiObject</param>
        /// <param name="scaleFrom">Start scale</param>
        /// <param name="scaleTo">Target scale</param>
        /// <param name="time">Tween time in seconds</param>
        /// <param name="type">Tween type</param>
        public UiTweenScale(UiObject target, float scaleFrom, float scaleTo, float time, TweenType type)
            : base(0, 1, time, type)
        {
            Setup(target.GraphicsTransform, scaleFrom, scaleTo);
        }

        /// <summary>
        /// Initializes a new instance of the UiTweenScale.
        /// </summary>
        /// <param name="target">Target GameObject</param>
        /// <param name="scaleFrom">Start scale</param>
        /// <param name="scaleTo">Target scale</param>
        /// <param name="time">Tween time in seconds</param>
        /// <param name="type">Tween type</param>
        public UiTweenScale(GameObject target, Vector2 scaleFrom, Vector2 scaleTo, float time, TweenType type)
            : base(0, 1, time, type)
        {
            Setup(target.GetComponent<RectTransform>(), scaleFrom, scaleTo);
        }

        /// <summary>
        /// Initializes a new instance of the UiTweenScale.
        /// </summary>
        /// <param name="target">Target GameObject</param>
        /// <param name="scaleFrom">Start scale</param>
        /// <param name="scaleTo">Target scale</param>
        /// <param name="time">Tween time in seconds</param>
        /// <param name="type">Tween type</param>
        public UiTweenScale(GameObject target, float scaleFrom, float scaleTo, float time, TweenType type)
            : base(0, 1, time, type)
        {
            Setup(target.GetComponent<RectTransform>(), scaleFrom, scaleTo);
        }

        // Setups tween
        private void Setup(RectTransform target, float scaleFrom, float scaleTo)
        {
            Setup
            (
                target,
                new Vector2(scaleFrom, scaleFrom),
                new Vector2(scaleTo, scaleTo)
            );
        }

        // Setups tween
        private void Setup(RectTransform target, Vector2 scaleFrom, Vector2 scaleTo)
        {
            _rectTransform = target;
            _scaleFrom = scaleFrom;
            _scaleTo = scaleTo;
        }

        // Tween iteration
        internal override void Update(float value)
        {
            float x = _scaleFrom.x + (_scaleTo.x - _scaleFrom.x) * value;
            float y = _scaleFrom.y + (_scaleTo.y - _scaleFrom.y) * value;

            _rectTransform.localScale = new Vector2(x, y);
        }
    }
}


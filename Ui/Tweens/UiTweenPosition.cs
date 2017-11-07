using UnityEngine;
using UnityExpansion.Tweens;

namespace UnityExpansion.UI.Tweens
{
    /// <summary>
    /// Provides linear change of Ui object's anchored position by specified time.
    /// </summary>
    /// <seealso cref="UnityExpansion.Tweens.TweenLineal" />
    public class UiTweenPosition : TweenLineal
    {
        private RectTransform _rectTransform;

        private Vector2 _positionFrom;
        private Vector2 _positionTo;

        /// <summary>
        /// Initializes a new instance of the UiTweenPosition.
        /// </summary>
        /// <param name="target">Target UiObject</param>
        /// <param name="positionFrom">Start position</param>
        /// <param name="positionTo">Target position</param>
        /// <param name="time">Tween time in seconds</param>
        /// <param name="type">Tween type</param>
        public UiTweenPosition(UiObject target, Vector2 positionFrom, Vector2 positionTo, float time, TweenType type)
            : base(0, 1, time, type)
        {
            Setup(target.GraphicsTransform, positionFrom, positionTo);
        }

        /// <summary>
        /// Initializes a new instance of the UiTweenPosition.
        /// </summary>
        /// <param name="target">Target GameObject</param>
        /// <param name="positionFrom">Start position</param>
        /// <param name="positionTo">Target position</param>
        /// <param name="time">Tween time in seconds</param>
        /// <param name="type">Tween type</param>
        public UiTweenPosition(GameObject target, Vector2 positionFrom, Vector2 positionTo, float time, TweenType type)
            : base(0, 1, time, type)
        {
            Setup(target.GetComponent<RectTransform>(), positionFrom, positionTo);
        }

        // Setups tween
        private void Setup(RectTransform target, Vector2 positionFrom, Vector2 positionTo)
        {
            _rectTransform = target;
            _positionFrom = positionFrom;
            _positionTo = positionFrom;
        }

        // Tween iteration
        internal override void Update(float value)
        {
            float x = _positionFrom.x + (_positionTo.x - _positionFrom.x) * value;
            float y = _positionFrom.y + (_positionTo.y - _positionFrom.y) * value;

            _rectTransform.anchoredPosition = new Vector2(x, y);
        }
    }
}


using System;

using UnityExpansion.Services;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Provides transition between two layout elements.
    /// Used by UiLayout and usually you don't need to use this class manualy.
    /// </summary>
    public class UiLayoutTransition
    {
        /// <summary>
        /// ElementA is hidden. Invokes at start if elementA is null.
        /// </summary>
        public event Action OnHide;

        /// <summary>
        /// Transition is completed. Invokes at start if both elements is null.
        /// </summary>
        public event Action OnComplete;

        private UiLayoutElement _elementA;
        private UiLayoutElement _elementB;

        private float _offsetTime;

        private bool _isStarted = false;

        /// <summary>
        /// Creates new transition between specified layout elements.
        /// </summary>
        /// <param name="elementA">Layout element that will be hidden</param>
        /// <param name="elementB">Layout element that will be shown</param>
        /// <param name="offsetTime">
        /// Normalized time of elementA's hide duration that the elementB will wait before start to showing.
        /// For example, a value of 0.5 would mean the elementB will begin showing at 50% of the elementA hide animation time.
        /// </param>
        /// <param name="onComplete">On complete callback</param>
        public UiLayoutTransition(UiLayoutElement elementA, UiLayoutElement elementB, float offsetTime)
        {
            _elementA = elementA;
            _elementB = elementB;

            _offsetTime = offsetTime;
        }

        /// <summary>
        /// Starts the transition.
        /// </summary>
        public void Start()
        { 
            if(_isStarted)
            {
                return;
            }

            _isStarted = true;

            // Transition not required
            if (_elementA == null && _elementB == null)
            {
                OnHide.InvokeIfNotNull();
                OnComplete.InvokeIfNotNull();

                return;
            }

            // Nothing to show
            if (_elementA != null && _elementB == null)
            {
                ElementAHide();
                return;
            }

            // Nothing to hide
            if(_elementA == null && _elementB != null)
            {
                ElementBShow();
                return;
            }

            float hideDuration = _elementA.GetAnimationHideDuration();

            if(_offsetTime <= 0 || hideDuration <= 0)
            {
                ElementAHideWithoutCallback();
                ElementBShow();
            }
            else if (_offsetTime >= 1)
            {
                ElementAHide();
            }
            else
            {
                ElementAHideWithoutCallback();

                new DeferredAction
                (
                    ElementBShow,
                    hideDuration * _offsetTime,
                    DeferredType.TimeBased
                );
            }
        }

        // Hides elementA
        private void ElementAHide()
        {
            _elementA.OnHide += ElementAHideCompleted;
            _elementA.Hide();
        }

        // Hides elementA with out handler
        private void ElementAHideWithoutCallback()
        {
            _elementA.OnHide += ElementAHideWithoutCallbackCompleted;
            _elementA.Hide();
        }

        // ElementA hide completed
        private void ElementAHideCompleted()
        {
            OnHide.InvokeIfNotNull();

            _elementA.OnHide -= ElementAHideCompleted;

            if (_elementB == null)
            {
                OnComplete.InvokeIfNotNull();
            }
            else
            {
                ElementBShow();
            }
        }

        // ElementA hide completed
        private void ElementAHideWithoutCallbackCompleted()
        {
            OnHide.InvokeIfNotNull();

            _elementA.OnHide -= ElementAHideWithoutCallbackCompleted;
        }

        // Shows elementB
        private void ElementBShow()
        {
            _elementB.OnShow += ElementBShowCompleted;
            _elementB.Show();
        }

        // ElementB show completed
        private void ElementBShowCompleted()
        {
            _elementB.OnShow -= ElementBShowCompleted;

            OnComplete.InvokeIfNotNull();
        }
    }
}
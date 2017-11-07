using System;

using UnityEngine;
using UnityExpansion.Services;

namespace UnityExpansion.Tweens
{
    /// <summary>
    /// Provides linear change of value by specified time.
    /// </summary>
    public class TweenLineal
    {
        /// <summary>
        /// Called after tween is finished.
        /// </summary>
        public Action OnComplete;

        /// <summary>
        /// Called on each iteration after value has been changed.
        /// </summary>
        public Action<float> OnValueChange;

        // Tween type
        private TweenType _type;

        // Tween time
        private float _time;

        // Start value
        private float _valueFrom;

        // Target value
        private float _valueTo;

        // Delta of start and target value
        private float _valueDelta;

        // Current value
        private float _valueCurrent;

        /// <summary>
        /// Initializes a new instance of the UiTween.
        /// </summary>
        /// <param name="valueFrom">Start value</param>
        /// <param name="valueTo">Target value</param>
        /// <param name="time">Tween time in seconds</param>
        /// <param name="type">Tween type</param>
        public TweenLineal(float valueFrom, float valueTo, float time, TweenType type)
        {
            _valueFrom = valueFrom;
            _valueTo = valueTo;
            _valueCurrent = valueFrom;

            _type = type;

            Singnals.AddListener(UnityExpansionIndex.SIGNAL_FRAME_START, Tick);
        }

        /// <summary>
        /// Stops the tween.
        /// </summary>
        public void Stop()
        {
            Singnals.RemoveListener(UnityExpansionIndex.SIGNAL_FRAME_START, Tick);
        }

        // Main iteration
        private void Tick()
        {
            float timeStep = UtilityTime.DeltaTime / _time;
            float step = _valueDelta * timeStep;

            if (_valueCurrent < _valueTo)
            {
                _valueCurrent += step;
            }

            if (_valueCurrent > _valueTo)
            {
                _valueCurrent -= step;
            }

            // Tween completed
            if (Mathf.Abs(_valueCurrent - _valueTo) <= step)
            {
                switch (_type)
                {
                    case TweenType.PlayOnce:
                        Finish();
                        break;

                    case TweenType.LoopSimple:
                        Restart();
                        break;

                    case TweenType.LoopPendulum:
                        Reverce();
                        break;
                }
            }
            else
            {
                Update(_valueCurrent);
                OnValueChange.InvokeIfNotNull(_valueCurrent);
            }
        }

        // Finish tween
        private void Finish()
        {
            Update(_valueCurrent);

            OnValueChange.InvokeIfNotNull(_valueTo);
            OnComplete.InvokeIfNotNull();

            Stop();
        }

        // Restart tween
        private void Restart()
        {
            _valueCurrent = _valueFrom;
        }

        // Reverse tween
        private void Reverce()
        {
            float temp = _valueFrom;

            _valueFrom = _valueTo;
            _valueTo = temp;
        }

        // Internal update
        internal virtual void Update(float value) { }

        // Internal complete
        internal virtual void Complete() { }
    }
}
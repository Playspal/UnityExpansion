using UnityExpansion.Utilities;

namespace UnityExpansion.Services
{
    /// <summary>
    /// Abstract class that provides deferred service functionality.
    /// </summary>
    public abstract class Deferred
    {
        private float _delay;
        private float _delayDefined;

        private bool _isStarted = false;

        private DeferredType _type;

        /// <summary>
        /// Starts the service. If the service was stoped before, it will be restarted and delay will be reseted.
        /// </summary>
        public void Start()
        {
            if(_isStarted)
            {
                return;
            }

            _isStarted = true;
            _delay = _delayDefined;

            Expansion.Instance.OnUpdate += Process;
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        public void Stop()
        {
            if (!_isStarted)
            {
                return;
            }

            _isStarted = false;

            Expansion.Instance.OnUpdate -= Process;
        }

        /// <summary>
        /// Starts the service. This method should be called from constructor of inheriting class.
        /// </summary>
        /// <param name="delay">Delay value in frames or seconds</param>
        /// <param name="type">Deferred service type</param>
        protected void SetupAndStart(float delay, DeferredType type)
        {
            _delay = delay;
            _delayDefined = delay;

            _type = type;

            Start();
        }

        // Makes countdown and performs callback when countdown is finished.
        private void Process()
        {
            // Countdown
            switch (_type)
            {
                case DeferredType.FramesBased:
                    _delay -= 1;
                    break;

                case DeferredType.TimeBased:
                    _delay -= UtilityTime.DeltaTime;
                    break;
            }

            // Execute callback
            if (_delay <= 0)
            {
                Stop();
                Invoke();
            }
        }

        /// <summary>
        /// Will be called after countdown finish.
        /// </summary>
        protected abstract void Invoke();
    }
}
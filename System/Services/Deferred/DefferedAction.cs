using System;
using UnityEngine;

namespace UnityExpansion.Services
{
    /// <summary>
    /// Executes action from the main thread with specified delay.
    /// </summary>
    public class DefferedAction
    {
        private float _delay;

        private DefferedActionType _type;
        private Action _callback;

        /// <summary>
        /// Initializes a new instance of the DefferedAction class with specified callback and delay
        /// </summary>
        /// <param name="callback">Callback that will be executed with delay</param>
        /// <param name="delay">Delay value in frames or seconds</param>
        /// <param name="type">Deffered action type</param>
        public DefferedAction(Action callback, float delay = 0, DefferedActionType type = DefferedActionType.FramesBased)
        {
            _callback = callback;
            _delay = delay;
            _type = type;

            // Subscribe to main thread iteration
            Singnals.AddListener(UnityExpansion.SIGNAL_FRAME_START, Process);
        }

        // Makes countdown and executes callback when countdown is finished
        private void Process()
        {
            // Countdown
            switch (_type)
            {
                case DefferedActionType.FramesBased:
                    _delay -= 1;
                    break;

                case DefferedActionType.TimeBased:
                    _delay -= Time.deltaTime;
                    break;
            }

            // Execute callback
            if (_delay <= 0)
            {
                _callback.InvokeIfNotNull();

                // Unsubscribe from main thread iteration
                Singnals.RemoveListener(UnityExpansion.SIGNAL_FRAME_START, Process);
            }
        }
    }
}
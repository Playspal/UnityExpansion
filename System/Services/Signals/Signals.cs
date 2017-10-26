using System;
using System.Collections.Generic;

namespace UnityExpansion.Services
{
    /// <summary>
    /// Provides basic events system functionality
    /// </summary>
    public static class Singnals
    {
        private class Signal
        {
            public string Name;
            public Action Handler;
        }

        // List of registered signals
        private static List<Signal> _signals = new List<Signal>();

        /// <summary>
        /// Dispatch signal with provided name
        /// </summary>
        /// <param name="name">Signal name</param>
        public static void Dispatch(string name)
        {
            Signal signal = GetSignal(name);
            signal.Handler.InvokeIfNotNull();
        }

        /// <summary>
        /// Subscribe on provided signal
        /// </summary>
        /// <param name="name">Signal name</param>
        /// <param name="handler">Signal handler</param>
        public static void AddListener(string name, Action handler)
        {
            Signal signal = GetSignal(name);
            signal.Handler += handler;
        }

        /// <summary>
        /// Unsubscribe from provided signal
        /// </summary>
        /// <param name="name">Signal name</param>
        /// <param name="handler">Signal handler</param>
        public static void RemoveListener(string name, Action handler)
        {
            Signal signal = GetSignal(name);
            signal.Handler -= handler;
        }

        // Searches for signal, creates new one if signal not found
        private static Signal GetSignal(string name)
        {
            Signal signal = _signals.Find(x => x.Name == name);

            if (signal == null)
            {
                signal = new Signal
                {
                    Name = name
                };

                _signals.Add(signal);
            }

            return signal;
        }
    }
}
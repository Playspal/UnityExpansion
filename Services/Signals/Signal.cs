using System;

namespace UnityExpansion.Services
{
    public class Signal
    {
        /// <summary>
        /// Unique signal name
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Creates new signal
        /// </summary>
        /// <param name="signalName">Unique signal name</param>
        public Signal(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Adds listener to signal.
        /// </summary>
        /// <param name="handler">Signal handler</param>
        public void AddListener(Action handler)
        {
            Signals.AddListener(Name, handler);
        }

        /// <summary>
        /// Removes listener from signal.
        /// </summary>
        /// <param name="handler">Signal handler</param>
        public void RemoveListener(Action handler)
        {
            Signals.RemoveListener(Name, handler);
        }

        /// <summary>
        /// Dispatches signal.
        /// </summary>
        public void Dispatch()
        {
            Signals.Dispatch(Name);
        }
    }
}
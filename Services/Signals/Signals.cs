using System;
using System.Collections.Generic;

namespace UnityExpansion.Services
{
    /// <summary>
    /// Provides basic events system functionality
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityExpansion.Services;
    /// 
    /// public class MyClass1
    /// {
    ///     public MyClass1()
    ///     {
    ///         // Subscribe to "mySignalName" signal
    ///         Signals.AddListener("mySignalName", MyMethod);
    ///     }
    ///     
    ///     private void MyMethod()
    ///     {
    ///         // Unsubscribe from "mySignalName" signal
    ///         Signals.RemoveListener("mySignalName", MyMethod);
    ///     }
    /// }
    /// 
    /// public class MyClass2
    /// {
    ///     public MyClass2()
    ///     {
    ///         // Dispatch "mySignalName"
    ///         Signals.Dispatch("mySignalName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public static class Signals
    {
        // Registered signal entry data structure.
        private class SignalEntry
        {
            public readonly string Signal;
            public Action Handler;

            public SignalEntry(string signal)
            {
                Signal = signal;
            }
        }

        // List of registered signals
        private static List<SignalEntry> _entries = new List<SignalEntry>();

        /// <summary>
        /// Dispatch group of signals.
        /// </summary>
        /// <param name="signals">Array of signal</param>
        public static void DispatchGroup(string[] signals)
        {
            for(int i = 0; i < signals.Length; i++)
            {
                Dispatch(signals[i]);
            }
        }

        /// <summary>
        /// Dispatches specified signal.
        /// </summary>
        /// <param name="signal">Signal</param>
        public static void Dispatch(string signal)
        {
            if (string.IsNullOrEmpty(signal))
            {
                return;
            }

            SignalEntry signalEntry = GetSignalEntry(signal);

            if (signalEntry.Handler != null)
            {
                Delegate[] delegates = signalEntry.Handler.GetInvocationList();

                for (int i = 0; i < delegates.Length; i++)
                {
                    delegates[i].DynamicInvoke();
                }
            }
        }

        /// <summary>
        /// Subscribe on specified signal.
        /// </summary>
        /// <param name="signal">Signal</param>
        /// <param name="handler">Signal handler</param>
        public static void AddListener(string signal, Action handler)
        {
            if(string.IsNullOrEmpty(signal))
            {
                return;
            }

            SignalEntry signalEntry = GetSignalEntry(signal);
            signalEntry.Handler += handler;
        }

        /// <summary>
        /// Unsubscribe from specified signal.
        /// </summary>
        /// <param name="signal">Signal</param>
        /// <param name="handler">Signal handler</param>
        public static void RemoveListener(string signal, Action handler)
        {
            if (string.IsNullOrEmpty(signal))
            {
                return;
            }

            SignalEntry signalEntry = GetSignalEntry(signal);
            signalEntry.Handler -= handler;
        }

        // Searches for entry, creates new one if entry not found
        private static SignalEntry GetSignalEntry(string signal)
        {
            SignalEntry signalEntry = _entries.Find(x => x.Signal == signal);

            if (signalEntry == null)
            {
                signalEntry = new SignalEntry(signal);

                _entries.Add(signalEntry);
            }

            return signalEntry;
        }
    }
}
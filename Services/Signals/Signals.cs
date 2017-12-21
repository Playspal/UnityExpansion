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
        // Signal structure class
        private class Signal
        {
            public string Name;
            public Action Handler;
        }

        // List of registered signals
        private static List<Signal> _signals = new List<Signal>();

        /// <summary>
        /// Dispatch group of signals with specified names.
        /// </summary>
        /// <param name="names">String that contains signals names</param>
        /// <param name="separator">Separator to split names string</param>
        public static void DispatchGroup(string names, char separator = ';')
        {
            DispatchGroup(names.Split(separator));
        }

        /// <summary>
        /// Dispatch group of signals with specified names.
        /// </summary>
        /// <param name="names">Array of signal names</param>
        public static void DispatchGroup(string[] names)
        {
            for(int i = 0; i < names.Length; i++)
            {
                Dispatch(names[i]);
            }
        }

        /// <summary>
        /// Dispatch signal with specified name.
        /// </summary>
        /// <param name="name">Signal name</param>
        public static void Dispatch(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Signal signal = GetSignal(name);

            if (signal.Handler != null)
            {
                Delegate[] delegates = signal.Handler.GetInvocationList();

                for (int i = 0; i < delegates.Length; i++)
                {
                    delegates[i].DynamicInvoke();
                }
            }
        }

        /// <summary>
        /// Subscribe on specified signal.
        /// </summary>
        /// <param name="name">Signal name</param>
        /// <param name="handler">Signal handler</param>
        public static void AddListener(string name, Action handler)
        {
            if(string.IsNullOrEmpty(name))
            {
                return;
            }

            Signal signal = GetSignal(name);
            signal.Handler += handler;
        }

        /// <summary>
        /// Unsubscribe from specified signal.
        /// </summary>
        /// <param name="name">Signal name</param>
        /// <param name="handler">Signal handler</param>
        public static void RemoveListener(string name, Action handler)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

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
using System;

namespace UnityExpansion.Services
{
    /// <summary>
    /// Executes action from the main thread with specified delay.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityExpansion.Services;
    /// 
    /// public class MyClass
    /// {
    ///     public MyClass()
    ///     {
    ///         // Executes MyMethod on begin of next frame
    ///         new DeferredAction(MyMethod);
    /// 
    ///         // Executes MeMethod on begin of 10th frame
    ///         new DeferredAction(MyMethod, 10, DeferredType.FramesBased);
    /// 
    ///         // Executes MeMethod with 1 second delay
    ///         new DeferredAction(MyMethod, 1, DeferredType.TimeBased);
    ///     }
    /// 
    ///     private void MyMethod() { }
    /// }
    /// </code>
    /// </example>
    public class DeferredAction : Deferred
    {
        private Action _callback;

        /// <summary>
        /// Initializes a new instance of the DeferredAction class with specified callback and delay.
        /// </summary>
        /// <param name="callback">Callback that will be executed with delay</param>
        /// <param name="delay">Delay value in frames or seconds</param>
        /// <param name="type">Deferred action type</param>
        public DeferredAction(Action callback, float delay = 0, DeferredType type = DeferredType.FramesBased)
        {
            _callback = callback;

            SetupAndStart(delay, type);
        }

        protected override void Invoke()
        {
            _callback.InvokeIfNotNull();
        }
    }
}
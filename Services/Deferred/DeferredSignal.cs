namespace UnityExpansion.Services
{
    /// <summary>
    /// Dispatches the signal with specified delay
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityExpansion.Services;
    /// 
    /// class MyClass
    /// {
    ///     public MyClass()
    ///     {
    ///         // Dispatches "MySignalName" signal after one second
    ///         new DeferredSignal("MySignalName", 1, DeferredType.TimeBased);
    ///     }
    /// }
    /// </code>
    /// </example>
    public class DeferredSignal : Deferred
    {
        private string _signalName;

        /// <summary>
        /// Initializes a new instance of the DeferredSignal class with specified sinal name and delay.
        /// </summary>
        /// <param name="signal">Signal name that will be dispatched with delay</param>
        /// <param name="delay">Delay value in frames or seconds</param>
        /// <param name="type">Delay type</param>
        public DeferredSignal(string signal, float delay = 0, DeferredType type = DeferredType.FramesBased)
        {
            _signalName = signal;

            SetupAndStart(delay, type);
        }

        internal override void Perform()
        {
            Singnals.Dispatch(_signalName);
        }
    }
}
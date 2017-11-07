namespace UnityExpansion.Services
{
    /// <summary>
    /// Defines deferred service countdown type.
    /// </summary>
    public enum DeferredType
    {
        /// <summary>
        /// Provides delay in frames/updates
        /// </summary>
        FramesBased,

        /// <summary>
        /// Provides delay in seconds
        /// </summary>
        TimeBased
    }
}
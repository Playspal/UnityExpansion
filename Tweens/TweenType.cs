namespace UnityExpansion.Tweens
{
    /// <summary>
    /// Defines loop type of tween.
    /// </summary>
    public enum TweenType
    {
        /// <summary>
        /// Play tween once
        /// </summary>
        PlayOnce,

        /// <summary>
        /// Play tween from start value to target and restart it
        /// </summary>
        LoopSimple,

        /// <summary>
        /// Fluctuate tween from start value to target and from target value to start
        /// </summary>
        LoopPendulum
    }
}
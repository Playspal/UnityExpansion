namespace UnityExpansion.Tweens
{
    /// <summary>
    /// Defines easing function that will be used for interpolation.
    /// </summary>
    public enum EasingType
    {
        /// <summary>
        /// Represents an easing function that creates an linear animation.
        /// </summary>
        Linear,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates using the formula f(t) = t^2
        /// </summary>
        QuadraticIn,

        /// <summary>
        /// Represents an easing function that creates an animation that decelerates using the formula f(t) = t^2
        /// </summary>
        QuadraticOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates and decelerates using the formula f(t) = t^2
        /// </summary>
        QuadraticInOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates using the formula f(t) = t^3.
        /// </summary>
        CubicIn,

        /// <summary>
        /// Represents an easing function that creates an animation that decelerates using the formula f(t) = t^3.
        /// </summary>
        CubicOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates and decelerates using the formula f(t) = t^3.
        /// </summary>
        CubicInOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates using the formula f(t) = t^4.
        /// </summary>
        QuarticIn,

        /// <summary>
        /// Represents an easing function that creates an animation that decelerates using the formula f(t) = t^4.
        /// </summary>
        QuarticOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates and decelerates using the formula f(t) = t^4.
        /// </summary>
        QuarticInOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates using the formula f(t) = t^5.
        /// </summary>
        QuinticIn,

        /// <summary>
        /// Represents an easing function that creates an animation that decelerates using the formula f(t) = t^5.
        /// </summary>
        QuinticOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates and decelerates using the formula f(t) = t^5.
        /// </summary>
        QuinticInOut,

        /// <summary>
        /// Creates an animation that accelerates using a sine formula. 
        /// </summary>
        SineIn,

        /// <summary>
        /// Creates an animation that decelerates using a sine formula. 
        /// </summary>
        SineOut,

        /// <summary>
        /// Creates an animation that accelerates and decelerates using a sine formula. 
        /// </summary>
        SineInOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates using a circular function.
        /// </summary>
        CircularIn,

        /// <summary>
        /// Represents an easing function that creates an animation that decelerates using a circular function.
        /// </summary>
        CircularOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates and decelerates using a circular function.
        /// </summary>
        CircularInOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates using an exponential formula.
        /// </summary>
        ExponentialIn,

        /// <summary>
        /// Represents an easing function that creates an animation that decelerates using an exponential formula.
        /// </summary>
        ExponentialOut,

        /// <summary>
        /// Represents an easing function that creates an animation that accelerates and decelerates using an exponential formula.
        /// </summary>
        ExponentialInOut,

        /// <summary>
        /// Represents an easing function that creates an animation that resembles a spring oscillating back and forth until it comes to rest.
        /// </summary>
        ElasticIn,

        /// <summary>
        /// Represents an easing function that creates an animation that resembles a spring oscillating back and forth until it comes to rest.
        /// </summary>
        ElasticOut,

        /// <summary>
        /// Represents an easing function that creates an animation that resembles a spring oscillating back and forth until it comes to rest.
        /// </summary>
        ElasticInOut,

        /// <summary>
        /// Represents an easing function that retracts the motion of an animation slightly before it begins to animate in the path indicated.
        /// </summary>
        BackIn,

        /// <summary>
        /// Represents an easing function that retracts the motion of an animation slightly before it begins to animate in the path indicated.
        /// </summary>
        BackOut,

        /// <summary>
        /// Represents an easing function that retracts the motion of an animation slightly before it begins to animate in the path indicated.
        /// </summary>
        BackInOut,

        /// <summary>
        /// Represents an easing function that creates an animated bouncing effect.
        /// </summary>
        BounceIn,

        /// <summary>
        /// Represents an easing function that creates an animated bouncing effect.
        /// </summary>
        BounceOut,

        /// <summary>
        /// Represents an easing function that creates an animated bouncing effect.
        /// </summary>
        BounceInOut
    }
}
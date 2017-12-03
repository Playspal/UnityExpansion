using UnityEngine;

namespace UnityExpansion.Tweens
{
    /// <summary>
    /// Provides interpolation based on predefined mathematical formulas.
    /// Also known as Robert Penner's Easing Functions.
    /// </summary>
    public static class Easing
    {
        /// <summary>
        /// Interpolates float between two values using the specified easing function.
        /// </summary>
        /// <param name="from">From value</param>
        /// <param name="to">To value</param>
        /// <param name="time">Normalized time</param>
        /// <param name="type">Easing type</param>
        /// <returns>Interpolated value</returns>
        public static float Interpolate(float from, float to, float time, EasingType type)
        {
            return from + (to - from) * Interpolate(time, type);
        }

        /// <summary>
        /// Interpolates Vector2 between two values using the specified easing function.
        /// </summary>
        /// <param name="from">From value</param>
        /// <param name="to">To value</param>
        /// <param name="time">Normalized time</param>
        /// <param name="type">Easing type</param>
        /// <returns>Interpolated value</returns>
        public static Vector2 Interpolate(Vector2 from, Vector2 to, float time, EasingType type)
        {
            return from + (to - from) * Interpolate(time, type);
        }

        /// <summary>
        /// Interpolates Vector3 between two values using the specified easing function.
        /// </summary>
        /// <param name="from">From value</param>
        /// <param name="to">To value</param>
        /// <param name="time">Normalized time</param>
        /// <param name="type">Easing type</param>
        /// <returns>Interpolated value</returns>
        public static Vector3 Interpolate(Vector3 from, Vector3 to, float time, EasingType type)
        {
            return from + (to - from) * Interpolate(time, type);
        }

        /// <summary>
        /// Interpolates Color between two values using the specified easing function.
        /// </summary>
        /// <param name="from">From value</param>
        /// <param name="to">To value</param>
        /// <param name="time">Normalized time</param>
        /// <param name="type">Easing type</param>
        /// <returns>Interpolated value</returns>
        public static Color Interpolate(Color from, Color to, float time, EasingType type)
        {
            return Color.Lerp(from, to, Interpolate(time, type));
        }

        /// <summary>
        /// Interpolate using the specified EasingType.
        /// </summary>
        /// <param name="time">Normalized time</param>
        /// <param name="type">Easing type</param>
        /// <returns>Interpolated value</returns>
        public static float Interpolate(float time, EasingType type)
        {
            switch (type)
            {
                default:
                case EasingType.Linear: return Linear(time);
                case EasingType.QuadraticOut: return QuadraticEaseOut(time);
                case EasingType.QuadraticIn: return QuadraticEaseIn(time);
                case EasingType.QuadraticInOut: return QuadraticEaseInOut(time);
                case EasingType.CubicIn: return CubicEaseIn(time);
                case EasingType.CubicOut: return CubicEaseOut(time);
                case EasingType.CubicInOut: return CubicEaseInOut(time);
                case EasingType.QuarticIn: return QuarticEaseIn(time);
                case EasingType.QuarticOut: return QuarticEaseOut(time);
                case EasingType.QuarticInOut: return QuarticEaseInOut(time);
                case EasingType.QuinticIn: return QuinticEaseIn(time);
                case EasingType.QuinticOut: return QuinticEaseOut(time);
                case EasingType.QuinticInOut: return QuinticEaseInOut(time);
                case EasingType.SineIn: return SineEaseIn(time);
                case EasingType.SineOut: return SineEaseOut(time);
                case EasingType.SineInOut: return SineEaseInOut(time);
                case EasingType.CircularIn: return CircularEaseIn(time);
                case EasingType.CircularOut: return CircularEaseOut(time);
                case EasingType.CircularInOut: return CircularEaseInOut(time);
                case EasingType.ExponentialIn: return ExponentialEaseIn(time);
                case EasingType.ExponentialOut: return ExponentialEaseOut(time);
                case EasingType.ExponentialInOut: return ExponentialEaseInOut(time);
                case EasingType.ElasticIn: return ElasticEaseIn(time);
                case EasingType.ElasticOut: return ElasticEaseOut(time);
                case EasingType.ElasticInOut: return ElasticEaseInOut(time);
                case EasingType.BackIn: return BackEaseIn(time);
                case EasingType.BackOut: return BackEaseOut(time);
                case EasingType.BackInOut: return BackEaseInOut(time);
                case EasingType.BounceIn: return BounceEaseIn(time);
                case EasingType.BounceOut: return BounceEaseOut(time);
                case EasingType.BounceInOut: return BounceEaseInOut(time);
            }
        }

        private static float Linear(float p)
        {
            return p;
        }

        private static float QuadraticEaseIn(float p)
        {
            return p * p;
        }

        private static float QuadraticEaseOut(float p)
        {
            return -(p * (p - 2));
        }

        private static float QuadraticEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                return 2 * p * p;
            }
            else
            {
                return (-2 * p * p) + (4 * p) - 1;
            }
        }

        private static float CubicEaseIn(float p)
        {
            return p * p * p;
        }

        private static float CubicEaseOut(float p)
        {
            float f = (p - 1);
            return f * f * f + 1;
        }

        private static float CubicEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                return 4 * p * p * p;
            }
            else
            {
                float f = ((2 * p) - 2);
                return 0.5f * f * f * f + 1;
            }
        }

        private static float QuarticEaseIn(float p)
        {
            return p * p * p * p;
        }

        private static float QuarticEaseOut(float p)
        {
            float f = (p - 1);
            return f * f * f * (1 - p) + 1;
        }

        private static float QuarticEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                return 8 * p * p * p * p;
            }
            else
            {
                float f = (p - 1);
                return -8 * f * f * f * f + 1;
            }
        }

        private static float QuinticEaseIn(float p)
        {
            return p * p * p * p * p;
        }

        private static float QuinticEaseOut(float p)
        {
            float f = (p - 1);
            return f * f * f * f * f + 1;
        }

        private static float QuinticEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                return 16 * p * p * p * p * p;
            }
            else
            {
                float f = ((2 * p) - 2);
                return 0.5f * f * f * f * f * f + 1;
            }
        }

        private static float SineEaseIn(float p)
        {
            return Mathf.Sin((p - 1) * (Mathf.PI / 2.0f)) + 1;
        }

        private static float SineEaseOut(float p)
        {
            return Mathf.Sin(p * (Mathf.PI / 2.0f));
        }

        private static float SineEaseInOut(float p)
        {
            return 0.5f * (1 - Mathf.Cos(p * Mathf.PI));
        }

        private static float CircularEaseIn(float p)
        {
            return 1 - Mathf.Sqrt(1 - (p * p));
        }

        private static float CircularEaseOut(float p)
        {
            return Mathf.Sqrt((2 - p) * p);
        }

        private static float CircularEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                return 0.5f * (1 - Mathf.Sqrt(1 - 4 * (p * p)));
            }
            else
            {
                return 0.5f * (Mathf.Sqrt(-((2 * p) - 3) * ((2 * p) - 1)) + 1);
            }
        }

        private static float ExponentialEaseIn(float p)
        {
            return (p == 0.0f) ? p : Mathf.Pow(2, 10 * (p - 1));
        }

        private static float ExponentialEaseOut(float p)
        {
            return (p == 1.0f) ? p : 1 - Mathf.Pow(2, -10 * p);
        }

        private static float ExponentialEaseInOut(float p)
        {
            if (p == 0.0 || p == 1.0) return p;

            if (p < 0.5f)
            {
                return 0.5f * Mathf.Pow(2, (20 * p) - 10);
            }
            else
            {
                return -0.5f * Mathf.Pow(2, (-20 * p) + 10) + 1;
            }
        }

        private static float ElasticEaseIn(float p)
        {
            return Mathf.Sin(13 * (Mathf.PI / 2.0f) * p) * Mathf.Pow(2, 10 * (p - 1));
        }

        private static float ElasticEaseOut(float p)
        {
            return Mathf.Sin(-13 * (Mathf.PI / 2.0f) * (p + 1)) * Mathf.Pow(2, -10 * p) + 1;
        }

        private static float ElasticEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                return 0.5f * Mathf.Sin(13 * (Mathf.PI / 2.0f) * (2 * p)) * Mathf.Pow(2, 10 * ((2 * p) - 1));
            }
            else
            {
                return 0.5f * (Mathf.Sin(-13 * (Mathf.PI / 2.0f) * ((2 * p - 1) + 1)) * Mathf.Pow(2, -10 * (2 * p - 1)) + 2);
            }
        }

        private static float BackEaseIn(float p)
        {
            return p * p * p - p * Mathf.Sin(p * Mathf.PI);
        }

        private static float BackEaseOut(float p)
        {
            float f = (1 - p);
            return 1 - (f * f * f - f * Mathf.Sin(f * Mathf.PI));
        }

        private static float BackEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                float f = 2 * p;
                return 0.5f * (f * f * f - f * Mathf.Sin(f * Mathf.PI));
            }
            else
            {
                float f = (1 - (2 * p - 1));
                return 0.5f * (1 - (f * f * f - f * Mathf.Sin(f * Mathf.PI))) + 0.5f;
            }
        }

        private static float BounceEaseIn(float p)
        {
            return 1 - BounceEaseOut(1 - p);
        }

        private static float BounceEaseOut(float p)
        {
            if (p < 4 / 11.0f)
            {
                return (121 * p * p) / 16.0f;
            }
            else if (p < 8 / 11.0f)
            {
                return (363 / 40.0f * p * p) - (99 / 10.0f * p) + 17 / 5.0f;
            }
            else if (p < 9 / 10.0f)
            {
                return (4356 / 361.0f * p * p) - (35442 / 1805.0f * p) + 16061 / 1805.0f;
            }
            else
            {
                return (54 / 5.0f * p * p) - (513 / 25.0f * p) + 268 / 25.0f;
            }
        }

        private static float BounceEaseInOut(float p)
        {
            if (p < 0.5f)
            {
                return 0.5f * BounceEaseIn(p * 2);
            }
            else
            {
                return 0.5f * BounceEaseOut(p * 2 - 1) + 0.5f;
            }
        }
    }
}
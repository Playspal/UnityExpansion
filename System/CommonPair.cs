namespace UnityExpansion
{
    /// <summary>
    /// Common class to keep combination of key and value.
    /// </summary>
    public class CommonPair<T1, T2>
    {
        /// <summary>
        /// The key.
        /// </summary>
        public T1 Key;

        /// <summary>
        /// The value.
        /// </summary>
        public T2 Value;

        /// <summary>
        /// Initializes a empty instance.
        /// </summary>
        public CommonPair() { }

        /// <summary>
        /// Initializes a key + value pair.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public CommonPair(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }
    }
}
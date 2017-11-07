using System.Collections.Generic;
using System.IO;

namespace UnityExpansion.IO
{
    /// <summary>
    /// Stores any kind of serializable data in a file. Use it instead of UnityEngine.PlayerPrefs.
    /// </summary>
    public static class Cache
    {
        private static List<CommonPair> _data = new List<CommonPair>();

        /// <summary>
        /// Initializes the cache
        /// </summary>
        static Cache()
        {
            Load();
        }

        /// <summary>
        /// Clears all cache data
        /// </summary>
        public static void Clear()
        {
            _data.Clear();
            Save();
        }

        /// <summary>
        /// Is cache contains specified key
        /// </summary>
        /// <param name="key">The key.</param>
        public static bool Contains(string key)
        {
            return _data.Exists(x => x.Key == key);
        }

        /// <summary>
        /// Sets the value of the preference identified by key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public static void Set(string key, object value)
        {
            CommonPair item = _data.Find(x => x.Key == key);

            if (item == null)
            {
                _data.Add(new CommonPair(key, value));
            }

            else
            {
                item.Value = value;
            }

            Save();
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">The key</param>
        /// <returns>Value corresponding to key or default(T)</returns>
        public static T Get<T>(string key)
        {
            return Get(key, default(T));
        }

        /// <summary>
        /// Returns the value corresponding to key in the preference file if it exists.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Value corresponding to key or default value</returns>
        public static T Get<T>(string key, T defaultValue)
        {
            CommonPair item = _data.Find(x => x.Key == key);

            if (item == null)
            {
                return defaultValue;
            }

            return (T)item.Value;
        }

        // Saves cache data
        private static void Save()
        {
            string filename = Environment.DataPath + "cache.dat";

            File.WriteAllText(filename, Serialization.ObjectToXML(_data));
        }

        // Loads cache data
        private static void Load()
        {
            string filename = Environment.DataPath + "cache.dat";

            if (File.Exists(filename))
            {
                string xml = File.ReadAllText(filename);
                _data = Serialization.XMLToObject(xml, _data.GetType()) as List<CommonPair>;
            }
        }
    }
}

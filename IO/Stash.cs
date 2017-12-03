﻿using System.Collections.Generic;
using System.IO;

using UnityExpansion.Utilities;

namespace UnityExpansion.IO
{
    /// <summary>
    /// Stores any kind of serializable data in a single file.
    /// Use it instead of UnityEngine.PlayerPrefs if you need to store generic types or complicated objects.
    /// </summary>
    /// <example>
    /// <code>
    /// using using System.Collections.Generic;
    /// using UnityExpansion.IO;
    /// 
    /// public class MyLeaderboard
    /// {
    ///     private const string STASH_KEY_BEST_PLAYERS = "BestPlayers";
    ///     private const string STASH_KEY_BEST_SCORE = "BestScore";
    ///     
    ///     public List<string> BestPlayers = new List<string>();
    ///     public int BestScore;
    ///     
    ///     public void Load()
    ///     {
    ///         // Gets data from stash
    ///         BestPlayers = Stash.Get(STASH_KEY_BEST_PLAYERS);
    ///         BestScore = Stash.Get(STASH_KEY_BEST_SCORE);
    ///     }
    ///     
    ///     public void Save()
    ///     {
    ///         // Puts data to stash
    ///         Stash.Set(STASH_KEY_BEST_PLAYERS, BestPlayers);
    ///         Stash.Set(STASH_KEY_BEST_SCORE, BestScore);
    ///     }
    /// }
    /// </code>
    /// </example>
    public static class Stash
    {
        private static List<CommonPair<string, object>> _data = new List<CommonPair<string, object>>();

        /// <summary>
        /// Initializes the cache.
        /// </summary>
        static Stash()
        {
            Load();
        }

        /// <summary>
        /// Clears all stash data.
        /// </summary>
        public static void Clear()
        {
            _data.Clear();
            Save();
        }

        /// <summary>
        /// Is stash contains specified key.
        /// </summary>
        /// <param name="key">The key</param>
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
            CommonPair<string, object> item = _data.Find(x => x.Key == key);

            if (item == null)
            {
                _data.Add(new CommonPair<string, object>(key, value));
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
            CommonPair<string, object> item = _data.Find(x => x.Key == key);

            if (item == null)
            {
                return defaultValue;
            }

            return (T)item.Value;
        }

        // Saves stash data
        private static void Save()
        {
            string filename = Environment.DataPath + "stash.dat";

            File.WriteAllText(filename, UtilitySerialization.ObjectToXML(_data));
        }

        // Loads stash data
        private static void Load()
        {
            string filename = Environment.DataPath + "stash.dat";

            if (File.Exists(filename))
            {
                string xml = File.ReadAllText(filename);
                _data = UtilitySerialization.XMLToObject(xml, _data.GetType()) as List<CommonPair<string, object>>;
            }
        }
    }
}

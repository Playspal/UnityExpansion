using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExpansion.Editor
{
    public class EditorLayoutObjectTextureCachable : EditorLayoutObjectTexture
    {
        private static Dictionary<string, Texture2D> _cache = new Dictionary<string, Texture2D>();

        private string _cacheID;

        public EditorLayoutObjectTextureCachable(EditorLayout layout, int width, int height, string cacheID) : base(layout, width, height)
        {
            SetCacheID(cacheID);
        }

        public void SetCacheID(string value)
        {
            _cacheID = value;
        }

        public bool LoadFromCache()
        {
            string id = GetCacheID();

            if (_cache.ContainsKey(id) && _cache[id] == null)
            {
                _cache.Remove(id);
            }

            if (_cache.ContainsKey(id))
            {
                Texture = _cache[id];
                return true;
            }

            return false;
        }

        public void SaveToCache()
        {
            string id = GetCacheID();

            if (!_cache.ContainsKey(id))
            {
                Texture2D copy = new Texture2D(Texture.width, Texture.height);
                Graphics.CopyTexture(Texture, copy);

                _cache.Add(id, copy);
            }
            else
            {
                Texture2D copy = new Texture2D(Texture.width, Texture.height);
                Graphics.CopyTexture(Texture, copy);

                _cache[id] = copy;
            }
        }

        private string GetCacheID()
        {
            return _cacheID + "." + Width + "." + Height;
        }

        public static void ClearCache()
        {
            _cache.Clear();
        }
    }
}
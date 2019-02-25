using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public static class UiLayoutEditorCache
    {
        private static Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

        public static Texture2D GetTexture(string id)
        {
            if(_textures.ContainsKey(id))
            {
                return _textures[id];
            }

            return null;
        }

        public static void AddTexture(string id, Texture2D texture)
        {
            Texture2D copy = new Texture2D(texture.width, texture.height);
            Graphics.CopyTexture(texture, copy);

            if (_textures.ContainsKey(id))
            {
                _textures[id] = copy;
            }
            else
            {
                _textures.Add(id, copy);
            }
        }
    }
}
using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLinkIcon : EditorLayoutObject
    {
        protected EditorLayoutObjectTextureCachable _texture;

        public NodeLinkIcon(EditorLayout layout) : base(layout, 15, 15)
        {
            _texture = new EditorLayoutObjectTextureCachable(layout, Width, Height, "node-link-icon");
            _texture.SetScale(4);
            _texture.SetParent(this);

            if (!_texture.LoadFromCache())
            {
                _texture.Fill(new Color(0, 0, 0, 0));
                _texture.DrawCircle(7, 7, 7, UiLayoutEditorConfig.ColorNodeBorder);
                _texture.DrawCircle(7, 7, 5, Color.white);
                _texture.SaveToCache();
            }
        }
    }
}
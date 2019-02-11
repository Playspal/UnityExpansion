using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLinkIcon : EditorLayoutObject
    {
        protected EditorLayoutObjectTexture _texture;

        public NodeLinkIcon(EditorLayout layout) : base(layout, 15, 15)
        {
            _texture = new EditorLayoutObjectTexture(layout, Width, Height);
            _texture.Fill(new Color(0, 0, 0, 0));
            _texture.DrawCircle(7, 7, 7, Color.white.Parse(UiLayoutEditorConfig.COLOR_NODE_BACKGROUND_BORDER));
            _texture.DrawCircle(7, 7, 5, Color.white);
            _texture.SetParent(this);
        }
    }
}
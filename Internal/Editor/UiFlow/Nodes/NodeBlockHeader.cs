using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeBlockHeader : EditorLayoutObject
    {
        private EditorLayoutTexture2D _textureBackground;
        private EditorLayoutObjectText _label;

        public NodeBlockHeader(EditorLayout layout, Node node) : base(layout, node.Width - 2, 30)
        {
            X = Y = 1;

            _textureBackground = new EditorLayoutTexture2D(layout, Width, Height);
            _textureBackground.Fill(node.ColorMain);
            _textureBackground.DrawBorderBottom(1, node.ColorDark);
            _textureBackground.SetParent(this);

            _label = new EditorLayoutObjectText(layout, Width - 18, Height);
            _label.SetFontStyle(FontStyle.Bold);
            _label.SetAlignment(TextAnchor.MiddleLeft);
            _label.SetColor(node.ColorLight);
            _label.SetText("...");
            _label.SetParent(this);
            _label.X = 9;
        }

        public void SetTitle(string value)
        {
            _label.SetText(value);
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
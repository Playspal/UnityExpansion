using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeBlockHeader : EditorLayoutObject
    {
        private Node _parentNode;

        private EditorLayoutObjectTextureCachable _textureBackground;
        private EditorLayoutObjectText _label;

        public NodeBlockHeader(EditorLayout layout, Node node) : base(layout, node.Width - 2, 30)
        {
            X = Y = 1;

            _parentNode = node;

            _textureBackground = new EditorLayoutObjectTextureCachable(layout, Width, Height, "node-header-" + node.ColorMain.ToString());
            _textureBackground.SetParent(this);

            if (!_textureBackground.LoadFromCache())
            {
                _textureBackground.Fill(node.ColorMain);
                _textureBackground.DrawBorderBottom(1, node.ColorDark);
                _textureBackground.SaveToCache();
            }

            _label = new EditorLayoutObjectText(layout, Width - 18, Height);
            _label.SetFontStyle(FontStyle.Bold);
            _label.SetAlignment(TextAnchor.MiddleLeft);
            _label.SetColor(node.ColorLight);
            _label.SetText("...");
            _label.SetParent(this);
            _label.X = 9;

            Layout.Mouse.OnPress += MouseHandlerPress;
            Layout.Mouse.OnRelease += MouseHandlerRelease;
        }

        public override void Destroy()
        {
            base.Destroy();

            Layout.Mouse.OnPress -= MouseHandlerPress;
            Layout.Mouse.OnRelease -= MouseHandlerRelease;
        }

        public void SetTitle(string value)
        {
            _label.SetText(value);
        }

        public override void Render()
        {
            base.Render();
        }

        private void MouseHandlerPress()
        {
            if (HitTest(Layout.Mouse.X, Layout.Mouse.Y))
            {
                _parentNode.DragStart(false);
            }
        }

        private void MouseHandlerRelease()
        {
            if (_parentNode.IsDragging)
            {
                _parentNode.DragStop();

                _parentNode.X = Mathf.RoundToInt((float)_parentNode.X / 20f) * 20;
                _parentNode.Y = Mathf.RoundToInt((float)(_parentNode.Y) / 20f) * 20;
            }
        }
    }
}
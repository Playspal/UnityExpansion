using UnityEngine;
using UnityExpansion.Editor;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeSystem : Node
    {
        private EditorLayoutObjectTexture _textureHeader;
        private EditorLayoutObjectText _title;
        private NodeConnector _connector;

        public NodeSystem(InternalUiLayoutData.NodeData nodeData, EditorLayout layout) : base(nodeData, layout, 200, 50)
        {
            _textureBackground = new EditorLayoutObjectTexture(layout, Width - 2, 6);
            _textureBackground.X = _textureBackground.Y = 1;
            _textureBackground.Fill(ColorMain);
            _textureBackground.DrawBorderBottom(1, ColorDark);
            _textureBackground.SetParent(this);

            _title = new EditorLayoutObjectText(layout, Width, Height - 6);
            _title.SetAlignment(TextAnchor.MiddleCenter);
            _title.SetFontStyle(FontStyle.Bold);
            _title.SetColor(ColorMain);
            _title.SetText("...");
            _title.SetParent(this);
            _title.Y = 5;

            Layout.Mouse.OnPress += MouseHandlerPress;
            Layout.Mouse.OnRelease += MouseHandlerRelease;
        }

        public override void Destroy()
        {
            base.Destroy();

            Layout.Mouse.OnPress -= MouseHandlerPress;
            Layout.Mouse.OnRelease -= MouseHandlerRelease;
        }

        protected void SetTitle(string value)
        {
            _title.SetText(value);
        }

        protected void SetConnector(NodeConnector connector)
        {
            _connector = connector;
        }

        private void MouseHandlerPress()
        {
            if (HitTest(Layout.Mouse.X, Layout.Mouse.Y) && !_connector.Icon.HitTest(Layout.Mouse.X, Layout.Mouse.Y))
            {
                DragStart(false);
            }
        }

        private void MouseHandlerRelease()
        {
            if (IsDragging)
            {
                DragStop();

                X = Mathf.RoundToInt((float)X / 20f) * 20;
                Y = Mathf.RoundToInt((float)Y / 20f) * 20;
            }
        }
    }
}
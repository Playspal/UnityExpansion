using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditorBackground
    {
        private EditorLayout _layout;
        private Texture2D _texture;

        public UiLayoutEditorBackground(EditorLayout layout)
        {
            _layout = layout;
            _layout.Mouse.OnDragByButtonRight += OnMouseDragByButtonRight;

            SetupTexture();
        }

        private void SetupTexture()
        {
            Color main = Color.white.Parse("#909090");
            Color line = Color.white.Parse("#7E7E7E");

            _texture = new Texture2D(120, 120);
            _texture.Fill(main);

            for (int x = 0; x < _texture.width; x += 20)
            {
                _texture.DrawRect(x, 0, 1, _texture.height, line);
            }

            for (int y = 0; y < _texture.height; y += 20)
            {
                _texture.DrawRect(0, y, _texture.width, 1, line);
            }
        }

        public void OnGUI()
        {
            int x = _layout.CanvasX % 20 - 20;
            int y = _layout.CanvasY % 20 - 20;

            for (int xx = x; xx < x + _layout.CanvasWidth + 40; xx += _texture.width)
            {
                for (int yy = y; yy < y + _layout.CanvasHeight + 40; yy += _texture.height)
                {
                    Rect bounds = new Rect(xx, yy, _texture.width, _texture.height);
                    GUI.DrawTexture(bounds, _texture, ScaleMode.StretchToFill, true, 1f);
                }
            }
        }

        private void OnMouseDragByButtonRight()
        {
            _layout.CanvasX += _layout.Mouse.DeltaX;
            _layout.CanvasY += _layout.Mouse.DeltaY;
        }
    }
}
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

        public void OnGUI()
        {
            int x = _layout.CanvasX % 20 - 20;
            int y = _layout.CanvasY % 20 - 20;
            Rect bounds = new Rect(x, y, _texture.width, _texture.height);
            GUI.DrawTexture(bounds, _texture, ScaleMode.StretchToFill, true, 1f);
        }

        private void SetupTexture()
        {
            Color main = Color.white.Parse("#909090");
            Color line = Color.white.Parse("#7E7E7E");

            _texture = new Texture2D(_layout.WindowWidth + 40, _layout.WindowHeight + 40);
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

        private void OnMouseDragByButtonRight()
        {
            _layout.CanvasX += _layout.Mouse.DeltaX;
            _layout.CanvasY += _layout.Mouse.DeltaY;
        }
    }
}
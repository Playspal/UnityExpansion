using UnityEngine;

namespace UnityExpansion.Editor
{
    public class EditorLayoutObjectText : EditorLayoutObject
    {
        public string Text { get; private set; }
        private Color _color = Color.black;
        private FontStyle _fontStyle = FontStyle.Normal;
        private TextAnchor _aligment = TextAnchor.UpperLeft;
        private int _fontSize = 11;

        private GUIStyle _style;

        private bool _styleUpdated;

        public EditorLayoutObjectText(EditorLayout window, int width, int height) : base(window, width, height)
        {
            Width = width;
            Height = height;
        }

        public void SetText(string value)
        {
            Text = value;
        }

        public void SetFontStyle(FontStyle fontStyle)
        {
            _fontStyle = fontStyle;
            _styleUpdated = true;
        }

        public void SetFontSize(int value)
        {
            _fontSize = value;
        }

        public void SetColor(string color)
        {
            SetColor(Color.white.Parse(color));
        }

        public void SetColor(Color color)
        {
            _color = color;
            _styleUpdated = true;
        }

        public void SetAlignment(TextAnchor textAnchor)
        {
            _aligment = textAnchor;
            _styleUpdated = true;
        }

        public override void Render()
        {
            base.Render();

            if(_style == null)
            {
                _style = new GUIStyle(GUI.skin.GetStyle("Label"));
                _style.fixedHeight = Height;
            }

            if(_styleUpdated)
            {
                _style.normal.textColor = _color;
                _style.fontStyle = _fontStyle;
                _style.alignment = _aligment;
                _style.fontSize = _fontSize;

                //Debug.LogError(_style.fontSize);
            }

            GUI.Label(new Rect(GlobalX, GlobalY, Width, Height), Text, _style);
        }
    }
}
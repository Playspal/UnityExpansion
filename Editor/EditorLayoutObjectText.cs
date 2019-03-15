using UnityEngine;

namespace UnityExpansion.Editor
{
    public class EditorLayoutObjectText : EditorLayoutObject
    {
        public string Text { get; private set; }

        public int PreferredWidth { get; private set; }
        public int PreferredHeight { get; private set; }

        private Color _color = Color.black;
        private FontStyle _fontStyle = FontStyle.Normal;
        private TextAnchor _aligment = TextAnchor.UpperLeft;
        private int _fontSize = 11;

        private GUIStyle _style;

        private bool _styleUpdated;

        public EditorLayoutObjectText(EditorLayout window, int width, int height) : base(window, width, height)
        {
            _style = new GUIStyle();
            _style.fixedHeight = Height;
            _style.padding = new RectOffset(2, 2, 1, 2);
        }

        public void SetText(string value)
        {
            Text = value;

            RefreshStyle();
            RecalculatePreferredSize();
        }

        public void SetFontStyle(FontStyle fontStyle)
        {
            _fontStyle = fontStyle;
            _styleUpdated = true;
        }

        public void SetFontSize(int value)
        {
            _fontSize = value;
            _styleUpdated = true;
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

            RefreshStyle();

            GUI.Label(new Rect(GlobalX, GlobalY, Width, Height), Text, _style);
        }

        private void RefreshStyle()
        {
            if (_styleUpdated)
            {
                _style.normal.textColor = _color;
                _style.fontStyle = _fontStyle;
                _style.alignment = _aligment;
                _style.fontSize = _fontSize;

                _styleUpdated = false;
            }
        }

        private void RecalculatePreferredSize()
        {
            GUIContent content = new GUIContent(Text);
            Vector2 size = _style.CalcSize(content);

            PreferredWidth = Mathf.RoundToInt(size.x);
            PreferredHeight = Mathf.RoundToInt(size.y);
        }
    }
}
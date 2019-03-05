using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExpansion.Editor
{
    public class EditorLayoutObjectText : EditorLayoutObject
    {
        private string _text = "Undefined";
        private Color _color = Color.black;
        private FontStyle _fontStyle = FontStyle.Normal;
        private TextAnchor _aligment = TextAnchor.UpperLeft;
        private GUIStyle _style;

        private bool _styleUpdated;

        public EditorLayoutObjectText(EditorLayout window, int width, int height) : base(window, width, height)
        {
            Width = width;
            Height = height;
        }

        public void SetText(string value)
        {
            _text = value;
        }

        public void SetFontStyle(FontStyle fontStyle)
        {
            _fontStyle = fontStyle;
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
            }

            GUI.Label(new Rect(GlobalX, GlobalY, Width, Height), _text, _style);
        }
    }
}
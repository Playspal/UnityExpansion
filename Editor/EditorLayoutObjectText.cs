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
        }

        public void SetColor(string color)
        {
            ColorUtility.TryParseHtmlString(color, out _color);
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void SetAlignment(TextAnchor textAnchor)
        {
            _aligment = textAnchor;
        }

        public override void Render()
        {
            base.Render();
            
            GUILayout.BeginArea(new Rect(GetPositionGlobalX(), GetPositionGlobalY(), Width, Height));

            GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));

            labelStyle.fixedHeight = Height;
            labelStyle.normal.textColor = _color;
            labelStyle.fontStyle = _fontStyle;
            labelStyle.alignment = _aligment;

            GUILayout.Label(_text, labelStyle);

            GUILayout.EndArea();
        }
    }
}
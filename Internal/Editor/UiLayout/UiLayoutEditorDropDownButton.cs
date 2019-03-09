using System;

using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditorDropDownButton : EditorLayoutObject
    {
        public event Action OnClick;

        private EditorLayoutObjectText _label;

        private UiLayoutEditorDropDownButtonIcon _icon;

        private Color _colorNormal;
        private Color _colorHover;

        public UiLayoutEditorDropDownButton(EditorLayout layout, int width, int height) : base(layout, width, height)
        {
            _icon = new UiLayoutEditorDropDownButtonIcon(Layout);
            _icon.SetParent(this);
            _icon.X = Width - _icon.Width;
            _icon.Y = Height / 2 - _icon.Height / 2;

            _label = new EditorLayoutObjectText(Layout, 0, Height);
            _label.SetAlignment(TextAnchor.MiddleRight);
            _label.SetFontStyle(FontStyle.Italic);
            _label.SetParent(this);

            _colorNormal = UiLayoutEditorConfig.COLOR_NODE_LABEL_SPECIAL;
            _colorHover = Color.white;

            SetColor(_colorNormal);

            SetupInteractions(_label);
            SetupInteractions(_icon);
        }

        public void SetLabel(string value)
        {
            _label.SetText(value);
            _label.SetSize(_label.PreferredWidth, Height);
            _label.X += _icon.X - _label.Width;
        }

        private void SetColor(Color color)
        {
            _label.SetColor(color);
            _icon.SetColor(color);
        }

        private void SetupInteractions(EditorLayoutObject layoutObject)
        {
            layoutObject.OnMouseReleaseInside += () =>
            {
                OnClick.InvokeIfNotNull();
            };

            layoutObject.OnMouseOver += () =>
            {
                SetColor(_colorHover);
            };

            layoutObject.OnMouseOut += () =>
            {
                SetColor(_colorNormal);
            };
        }
    }
}
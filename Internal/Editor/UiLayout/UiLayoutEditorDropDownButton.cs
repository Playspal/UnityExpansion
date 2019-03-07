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
            _icon.X = Width - 3 - _icon.Width;
            _icon.Y = Height / 2 - _icon.Height / 2;

            _label = new EditorLayoutObjectText(Layout, _icon.X - 5, Height);
            _label.SetAlignment(TextAnchor.MiddleRight);
            _label.SetText("Without animation");
            _label.SetColor(Color.white);
            _label.SetFontStyle(FontStyle.Italic);
            _label.SetParent(this);
            _label.X = 3;

            //Layout.Mouse.OnMove += MouseHandlerMove;
            //Layout.Mouse.OnClickLeft += MouseHandlerClick;
        }

        public void SetColor(Color color)
        {
            _colorNormal = color;

            _label.SetColor(color);
            _icon.SetColor(color);
        }

        private void SetColorNormal()
        {
            SetColor(_colorNormal);
        }

        private void SetColorWhite()
        {
            _label.SetColor(Color.white);
            _icon.SetColor(Color.white);
        }

        private void MouseHandlerMove()
        {
            if (HitTest(Layout.Mouse.X, Layout.Mouse.Y))
            {
                SetColorWhite();
            }
            else
            {
                SetColorNormal();
            }
        }

        private void MouseHandlerClick()
        {
            if (HitTest(Layout.Mouse.X, Layout.Mouse.Y))
            {
                OnClick.InvokeIfNotNull();
            }
        }

        public override void OnMouseOver()
        {
            base.OnMouseOver();
            SetColorWhite();
        }

        public override void OnMouseOut()
        {
            base.OnMouseOut();
            SetColorNormal();
        }
    }
}
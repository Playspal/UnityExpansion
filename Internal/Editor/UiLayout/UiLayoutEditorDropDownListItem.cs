using System;

using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditorDropDownListItem : EditorLayoutObject
    {
        public event Action<UiLayoutEditorDropDownListItem> OnClick;

        public readonly string Title;
        public readonly object Value;

        private EditorLayoutObjectText _label;

        public UiLayoutEditorDropDownListItem(EditorLayout layout, int width, string title, object value) : base(layout, width, 20)
        {
            Title = title;
            Value = value;

            _label = new EditorLayoutObjectText(Layout, Width - 4, Height);
            _label.SetAlignment(TextAnchor.MiddleRight);
            _label.SetText(Title);
            _label.SetColor(UiLayoutEditorConfig.ColorDropDownListItemLabelNormal);
            _label.SetParent(this);

            OnMouseOver += () =>
            {
                _label.SetColor(UiLayoutEditorConfig.ColorDropDownListItemLabelHover);
            };

            OnMouseOut += () =>
            {
                _label.SetColor(UiLayoutEditorConfig.ColorDropDownListItemLabelNormal);
            };

            OnMouseReleaseInside += () =>
            {
                OnClick.InvokeIfNotNull(this);
            };
        }
    }
}
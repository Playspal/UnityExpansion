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
            _label.SetColor(UiLayoutEditorConfig.DropDownListItemLabelNormal);
            _label.SetParent(this);

            OnMouseOver += () =>
            {
                _label.SetColor(UiLayoutEditorConfig.DropDownListItemLabelHover);
            };

            OnMouseOut += () =>
            {
                _label.SetColor(UiLayoutEditorConfig.DropDownListItemLabelNormal);
            };

            OnMouseReleaseInside += () =>
            {
                OnClick.InvokeIfNotNull(this);
            };
        }
    }
}
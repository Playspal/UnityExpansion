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

            _label = new EditorLayoutObjectText(Layout, Width, Height);
            _label.SetAlignment(TextAnchor.MiddleRight);
            _label.SetText(Title);
            _label.SetColor(Color.white);
            _label.SetParent(this);
        }
    }
}
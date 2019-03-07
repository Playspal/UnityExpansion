using System.Collections.Generic;

using UnityEngine;
using UnityExpansion;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditorDropDownList : EditorLayoutObject
    {
        private readonly EditorLayoutObjectTextureCachable _textureBackground;
        private List<UiLayoutEditorDropDownListItem> _items = new List<UiLayoutEditorDropDownListItem>();

        public UiLayoutEditorDropDownList(EditorLayout layout) : base(layout, 200, 400)
        {
            _textureBackground = new EditorLayoutObjectTextureCachable(layout, Width, Height, "drop-down-background");
            _textureBackground.SetParent(this);

            if (!_textureBackground.LoadFromCache())
            {
                _textureBackground.Fill(Color.red.Parse(UiLayoutEditorConfig.COLOR_NODE_BACKGROUND));
                _textureBackground.DrawBorder(1, Color.red.Parse(UiLayoutEditorConfig.COLOR_NODE_BACKGROUND_BORDER));
                _textureBackground.SaveToCache();
            }
        }

        public void SetData(List<CommonPair<string, object>> data)
        {
            int height = 0;

            for(int i = 0; i < data.Count; i++)
            {
                UiLayoutEditorDropDownListItem item = new UiLayoutEditorDropDownListItem(Layout, Width, data[i].Key, data[i].Value);

                item.SetParent(this);
                item.Y = i * item.Height;

                _items.Add(item);

                height = (i + 1) * item.Height;
            }

            SetSize(Width, height);
        }

        public override void SetSize(int width, int height)
        {
            base.SetSize(width, height);

            _textureBackground.SetSize(Width, Height);

            if (!_textureBackground.LoadFromCache())
            {
                _textureBackground.Fill(Color.red.Parse(UiLayoutEditorConfig.COLOR_NODE_BACKGROUND));
                _textureBackground.DrawBorder(1, Color.red.Parse(UiLayoutEditorConfig.COLOR_NODE_BACKGROUND_BORDER));
                _textureBackground.SaveToCache();
            }
        }
    }
}
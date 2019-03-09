using System;
using System.Collections.Generic;

using UnityEngine;
using UnityExpansion;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditorDropDownList : EditorLayoutObject
    {
        public event Action<object> OnSelect;

        private readonly EditorLayoutObjectTextureCachable _textureBackground;
        private readonly EditorLayoutObjectTextureCachable _textureShadow;

        private List<UiLayoutEditorDropDownListItem> _items = new List<UiLayoutEditorDropDownListItem>();

        public UiLayoutEditorDropDownList(EditorLayout layout) : base(layout, 200, 400)
        {
            _textureShadow = new EditorLayoutObjectTextureCachable(layout, Width, Height, "drop-down-shadow");
            _textureShadow.SetParent(this);
            _textureShadow.X = _textureShadow.Y = 3;

            if (!_textureShadow.LoadFromCache())
            {
                _textureShadow.Fill(new Color(0, 0, 0, 0.15f));
                _textureShadow.SaveToCache();
            }

            _textureBackground = new EditorLayoutObjectTextureCachable(layout, Width, Height, "drop-down-background");
            _textureBackground.SetParent(this);

            if (!_textureBackground.LoadFromCache())
            {
                _textureBackground.Fill(Color.red.Parse(UiLayoutEditorConfig.COLOR_NODE_BACKGROUND));
                _textureBackground.DrawBorder(1, Color.red.Parse(UiLayoutEditorConfig.COLOR_NODE_BACKGROUND_BORDER));
                _textureBackground.SaveToCache();
            }

            Layout.Mouse.OnPress += OnMouseClick;
        }

        public void SetData(List<CommonPair<string, object>> data)
        {
            int y = 4;

            for(int i = 0; i < data.Count; i++)
            {
                UiLayoutEditorDropDownListItem item = new UiLayoutEditorDropDownListItem(Layout, Width, data[i].Key, data[i].Value);

                item.SetParent(this);
                item.OnClick += OnItemClick;
                item.Y = y;

                y += item.Height + 2;

                _items.Add(item);
            }

            y += 2;

            SetSize(Width, y);
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

            _textureShadow.SetSize(Width, Height);

            if (!_textureShadow.LoadFromCache())
            {
                _textureShadow.Fill(new Color(0, 0, 0, 0.5f));
                _textureShadow.SaveToCache();
            }
        }

        private void OnMouseClick()
        {
            if (!HitTest(Layout.Mouse.X, Layout.Mouse.Y))
            {
                Destroy();
            }
        }

        private void OnItemClick(UiLayoutEditorDropDownListItem item)
        {
            OnSelect(item.Value);
            Destroy();
        }
    }
}
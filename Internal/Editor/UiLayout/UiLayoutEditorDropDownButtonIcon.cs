using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditorDropDownButtonIcon : EditorLayoutObject
    {
        private EditorLayoutObjectTextureCachable _triangleDown;

        public UiLayoutEditorDropDownButtonIcon(EditorLayout layout) : base(layout, 12, 5)
        {
            _triangleDown = new EditorLayoutObjectTextureCachable(Layout, 12, 5, "ui-layout-editor-drop-down-button-triangle-down-raw");
            _triangleDown.SetParent(this);
        }

        public void SetColor(Color color)
        {
            DrawArrowDown(color);
        }

        private void DrawArrowDown(Color color)
        {
            _triangleDown.SetCacheID("ui-layout-editor-drop-down-button-triangle-down-" + color.ToString());

            if (!_triangleDown.LoadFromCache())
            {
                // TODO: set size used to create new instance?!
                _triangleDown.SetSize(_triangleDown.Width, _triangleDown.Height);
                _triangleDown.Fill(new Color(0, 0, 0, 0));
                _triangleDown.DrawTriangleDown(5, 0, 5, color);
                _triangleDown.SaveToCache();
            }
        }
    }
}
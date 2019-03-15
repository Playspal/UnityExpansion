using UnityExpansion.Editor;
using UnityExpansion.UI.Layout;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLayoutElementRoot : NodeLayoutElement
    {
        private readonly EditorLayoutObjectText _label;
        private readonly UiLayoutEditorDropDownButton _button;

        public NodeLayoutElementRoot(InternalUiLayoutData.NodeData nodeData, EditorLayout layout, UiLayoutElement layoutElementt) : base(nodeData, layout, layoutElementt)
        {
            _label = new EditorLayoutObjectText(layout, Width, 20);
            _label.SetAlignment(UnityEngine.TextAnchor.MiddleLeft);
            _label.SetColor(UiLayoutEditorConfig.ColorNodeLabel);
            _label.SetText("Container");
            _label.SetParent(this);
            _label.Y = BlockHeader.Y + BlockHeader.Height + 8;
            _label.X = 10;

            _button = new UiLayoutEditorDropDownButton(Layout, Width - 12, 18);
            //_button.OnClick += OnButtonClick;
            _button.SetParent(this);
            _button.SetLabel("Layout");
            _button.X = 6;
            _button.Y = BlockHeader.Y + BlockHeader.Height + 8;

            _groupsOffset = _button.Height + 2 + 5;

            Refresh();
        }
    }
}
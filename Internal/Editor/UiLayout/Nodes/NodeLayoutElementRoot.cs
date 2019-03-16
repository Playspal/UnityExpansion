using System.Collections.Generic;
using UnityEngine;
using UnityExpansion;
using UnityExpansion.Editor;
using UnityExpansion.UI.Layout;
using UnityExpansion.UI.Layout.Processor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLayoutElementRoot : NodeLayoutElement
    {
        private readonly EditorLayoutObjectText _label;
        private readonly UiLayoutEditorDropDownButton _button;

        private readonly UiLayoutProcessorPreset _preset;

        public NodeLayoutElementRoot(InternalUiLayoutData.NodeData nodeData, EditorLayout layout, UiLayoutElement layoutElement) : base(nodeData, layout, layoutElement)
        {
            UiLayoutEditorSelection selection = ((UiLayoutEditor)Layout).Selection;

            _preset = selection.ProcessorPresetFind(NodeData.LayoutPresetID);

            _label = new EditorLayoutObjectText(layout, Width, 20);
            _label.SetAlignment(TextAnchor.MiddleLeft);
            _label.SetColor(UiLayoutEditorConfig.ColorNodeLabel);
            _label.SetText("Container");
            _label.SetParent(this);
            _label.Y = BlockHeader.Y + BlockHeader.Height + 8;
            _label.X = 10;

            _button = new UiLayoutEditorDropDownButton(Layout, Width - 12, 18);
            _button.OnClick += OnContainerButtonClick;
            _button.SetParent(this);
            _button.X = 6;
            _button.Y = BlockHeader.Y + BlockHeader.Height + 8;

            _groupsOffset = _button.Height + 2 + 5;

            Refresh();
        }

        private void OnContainerButtonClick()
        {
            UiLayoutEditorSelection selection = ((UiLayoutEditor)Layout).Selection;

            // Prepare data
            List<CommonPair<string, object>> data = new List<CommonPair<string, object>>();

            for (int i = 0; i < selection.Containers.Count; i++)
            {
                RectTransform rectTransform = selection.Containers[i];

                data.Add
                (
                    new CommonPair<string, object>(rectTransform.gameObject.name, rectTransform)
                );
            }

            // Show dropdown list
            UiLayoutEditorDropDownList list = new UiLayoutEditorDropDownList(Layout);

            list.SetData(data);
            list.X = _button.GlobalX + _button.Width - list.Width - Layout.CanvasX;
            list.Y = _button.GlobalY - Layout.CanvasY;
            list.OnSelect += (object selected) =>
            {
                _preset.Container = selected as RectTransform;
            };
        }

        public override void Render()
        {
            base.Render();

            _button.SetLabel(_preset.Container.name);
        }
    }
}
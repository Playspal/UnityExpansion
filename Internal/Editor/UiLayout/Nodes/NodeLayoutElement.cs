using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLayoutElement : Node
    {
        public readonly NodeBlockHeader BlockHeader;
        public readonly NodeBlockShowAndHide BlockShowAndHide;

        public UiLayoutElement LayoutElement { get; private set; }
        public UiLayoutPreset LayoutPreset { get; private set; }

        public NodeLayoutElement(EditorLayout layout) : base(layout, 300, 90)
        {
            BlockHeader = new NodeBlockHeader(layout, this);
            BlockHeader.SetParent(this);

            BlockShowAndHide = new NodeBlockShowAndHide(layout, this);
            BlockShowAndHide.SetParent(this);
            BlockShowAndHide.Y = 30;

            BlockShowAndHide.InputShow.OnConnected += (NodeConnector nodeConnector) =>
            {
                if (LayoutPreset != null)
                {
                    LayoutPreset.SignalsShow = UiLayoutEditorUtils.SignalsAdd(LayoutPreset.SignalsShow, nodeConnector.Data);
                }
            };

            BlockShowAndHide.InputShow.OnDisconnected += (NodeConnector nodeConnector) =>
            {
                if (LayoutPreset != null)
                {
                    LayoutPreset.SignalsShow = UiLayoutEditorUtils.SignalsRemove(LayoutPreset.SignalsShow, nodeConnector.Data);
                }
            };

            BlockShowAndHide.InputHide.OnConnected += (NodeConnector nodeConnector) =>
            {
                if (LayoutPreset != null)
                {
                    LayoutPreset.SignalsHide = UiLayoutEditorUtils.SignalsAdd(LayoutPreset.SignalsHide, nodeConnector.Data);
                }
            };

            BlockShowAndHide.InputHide.OnDisconnected += (NodeConnector nodeConnector) =>
            {
                if (LayoutPreset != null)
                {
                    LayoutPreset.SignalsHide = UiLayoutEditorUtils.SignalsRemove(LayoutPreset.SignalsHide, nodeConnector.Data);
                }
            };
        }

        public override void SetAsRootNode()
        {
            base.SetAsRootNode();

            BlockShowAndHide.InputShow.Label.SetText("Instantiate and Show");
            BlockShowAndHide.InputHide.Label.SetText("Hide and Destroy");
        }

        public void SetLayoutElement(UiLayoutElement layoutElement)
        {
            LayoutElement = layoutElement;

            BlockHeader.SetTitle(LayoutElement.name);
        }

        public void SetLayoutPreset(UiLayoutPreset layoutPreset)
        {
            LayoutPreset = layoutPreset;
        }
    }
}
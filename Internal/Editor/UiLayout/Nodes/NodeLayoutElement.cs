using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExpansion.Editor;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLayoutElement : Node
    {
        public readonly NodeBlockHeader BlockHeader;
        public readonly NodeBlockShowAndHide BlockShowAndHide;
        public readonly NodeBlockAnimation BlockAnimation;

        public UiLayoutElement LayoutElement { get; private set; }

        public NodeLayoutElement(InternalUiLayoutData.NodeData nodeData, EditorLayout layout) : base(nodeData, layout, 300, 90)
        {
            BlockHeader = new NodeBlockHeader(layout, this);
            BlockHeader.SetParent(this);

            BlockShowAndHide = new NodeBlockShowAndHide(layout, this);
            BlockShowAndHide.SetParent(this);
            BlockShowAndHide.Y = BlockHeader.Y + BlockHeader.Height + 10;

            BlockAnimation = new NodeBlockAnimation(layout, this);
            BlockAnimation.SetParent(this);
            BlockAnimation.Y = BlockShowAndHide.Y + BlockShowAndHide.Height + 10;

            BlockShowAndHide.InputShow.OnConnected += (NodeConnector nodeConnector) =>
            {
                if(LayoutElement != null)
                {
                    LayoutElement.SignalsShow = UiLayoutEditorUtils.SignalsAdd(LayoutElement.SignalsShow, nodeConnector.Data);
                    EditorUtility.SetDirty(LayoutElement);
                }
            };

            BlockShowAndHide.InputShow.OnDisconnected += (NodeConnector nodeConnector) =>
            {
                if (LayoutElement != null)
                {
                    LayoutElement.SignalsShow = UiLayoutEditorUtils.SignalsRemove(LayoutElement.SignalsShow, nodeConnector.Data);
                    EditorUtility.SetDirty(LayoutElement);
                }
            };

            BlockShowAndHide.InputHide.OnConnected += (NodeConnector nodeConnector) =>
            {
                if (LayoutElement != null)
                {
                    LayoutElement.SignalsHide = UiLayoutEditorUtils.SignalsAdd(LayoutElement.SignalsHide, nodeConnector.Data);
                    EditorUtility.SetDirty(LayoutElement);
                }
            };

            BlockShowAndHide.InputHide.OnDisconnected += (NodeConnector nodeConnector) =>
            {
                if (LayoutElement != null)
                {
                    LayoutElement.SignalsHide = UiLayoutEditorUtils.SignalsRemove(LayoutElement.SignalsHide, nodeConnector.Data);
                    EditorUtility.SetDirty(LayoutElement);
                }
            };

            SetSize(Width, BlockAnimation.Y + BlockAnimation.Height + 10);
        }

        public void SetLayoutElement(UiLayoutElement layoutElement)
        {
            LayoutElement = layoutElement;

            BlockHeader.SetTitle(LayoutElement.name);

            BlockShowAndHide.OutputOnShow.SetData(ID + "OnShow");
            BlockShowAndHide.OutputOnHide.SetData(ID + "OnHide");

            BlockShowAndHide.InputShow.SetData(LayoutElement.SignalsShow);
            BlockShowAndHide.InputHide.SetData(LayoutElement.SignalsHide);
        }
    }
}
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

            SetSize(Width, BlockAnimation.Y + BlockAnimation.Height + 10);
        }

        public void SetLayoutElement(UiLayoutElement layoutElement)
        {
            LayoutElement = layoutElement;

            BlockHeader.SetTitle(LayoutElement.name + " " + layoutElement.UniqueID);

            BlockShowAndHide.OutputOnShow.SetData(layoutElement.UniqueID, "OnShow");
            BlockShowAndHide.OutputOnHide.SetData(layoutElement.UniqueID, "OnHide");

            BlockShowAndHide.InputShow.SetData(layoutElement.UniqueID, "Show");
            BlockShowAndHide.InputHide.SetData(layoutElement.UniqueID, "Hide");
        }
    }
}
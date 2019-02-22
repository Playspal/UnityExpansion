using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExpansion.Editor;
using UnityExpansion.UI;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLayoutElement : Node
    {
        public readonly NodeBlockHeader BlockHeader;
        public readonly NodeBlockAnimation BlockAnimation;

        public UiLayoutElement LayoutElement { get; private set; }

        private List<NodeBlockGroup> _groups = new List<NodeBlockGroup>();

        public NodeLayoutElement(InternalUiLayoutData.NodeData nodeData, EditorLayout layout) : base(nodeData, layout, 300, 90)
        {
            BlockHeader = new NodeBlockHeader(layout, this);
            BlockHeader.SetParent(this);

            BlockAnimation = new NodeBlockAnimation(layout, this);
            BlockAnimation.SetParent(this);
            BlockAnimation.Y = BlockHeader.Y + BlockHeader.Height + 10;
            BlockAnimation.SetActive(false);
        }

        public void SetLayoutElement(UiLayoutElement layoutElement)
        {
            LayoutElement = layoutElement;

            BlockHeader.SetTitle(LayoutElement.name + " " + layoutElement.UniqueID + " " + layoutElement.GetInstanceID().ToString());

            if(layoutElement.GetComponent<UiAnimation>() != null)
            {
                BlockAnimation.SetActive(true);
                BlockAnimation.SetAnimation(layoutElement, layoutElement.GetComponent<UiAnimation>());
            }

            string[] outputs = UnityExpansion.Utilities.UtilityReflection.GetMethodsWithAttribute(layoutElement, typeof(UiLayoutMethod));

            for (int i = 0; i < outputs.Length; i++)
            {
                UiLayoutMethod a = UnityExpansion.Utilities.UtilityReflection.GetMethodAttribute(layoutElement, outputs[i], typeof(UiLayoutMethod)) as UiLayoutMethod;
                NodeBlockGroup blockGroup = GetGroup(a.Group);

                blockGroup.AddInput(LayoutElement.UniqueID, outputs[i], a.Order);
            }

            string[] inputs = UnityExpansion.Utilities.UtilityReflection.GetEventsWithAttribute(layoutElement, typeof(UiLayoutEvent));

            for (int i = 0; i < inputs.Length; i++)
            {
                UiLayoutEvent a = UnityExpansion.Utilities.UtilityReflection.GetEventAttribute(layoutElement, inputs[i], typeof(UiLayoutEvent)) as UiLayoutEvent;
                NodeBlockGroup blockGroup = GetGroup(a.Group);

                blockGroup.AddOutput(LayoutElement.UniqueID, inputs[i], a.Order);
            }

            Refresh();
        }

        private NodeBlockGroup GetGroup(string name)
        {
            NodeBlockGroup group = _groups.Find(x => x.GroupName == name);

            return group != null ? group : CreateGroup(name);
        }

        private NodeBlockGroup CreateGroup(string name)
        {
            NodeBlockGroup group = new NodeBlockGroup(Layout, this, name);
            group.SetParent(this);

            _groups.Add(group);

            return group;
        }

        private void Refresh()
        {
            int padding = 10;
            int y = BlockHeader.Y + BlockHeader.Height + padding;


            for (int i = 0; i < _groups.Count; i++)
            {
                if (_groups[i].IsMainGroup)
                {
                    _groups[i].Y = y;
                    y += _groups[i].Height + padding;

                    break;
                }
            }

            if (BlockAnimation.IsActive)
            {
                BlockAnimation.Y = y;
                y += BlockAnimation.Height + padding;
            }

            for (int i = 0; i < _groups.Count; i++)
            {
                if (!_groups[i].IsMainGroup)
                {
                    _groups[i].Y = y;
                    y += _groups[i].Height + padding;
                }
            }

            SetSize(Width, y);
        }
    }
}
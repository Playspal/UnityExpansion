using System.Collections.Generic;

using UnityExpansion.Editor;

using UnityExpansion.UI.Animation;
using UnityExpansion.UI.Layout;
using UnityExpansion.UI.Layout.Processor;
using UnityExpansion.Utilities;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLayoutElement : Node
    {
        public readonly NodeBlockHeader BlockHeader;
        public readonly NodeBlockAnimation BlockAnimation;

        public readonly UiLayoutElement LayoutElement;
        public readonly UiAnimation Animation;

        private List<NodeBlockGroup> _groups = new List<NodeBlockGroup>();

        public NodeLayoutElement(InternalUiLayoutData.NodeData nodeData, EditorLayout layout, UiLayoutElement layoutElement) : base(nodeData, layout, 300, 1)
        {
            LayoutElement = layoutElement;
            Animation = LayoutElement.GetComponent<UiAnimation>();

            BlockHeader = new NodeBlockHeader(layout, this);
            BlockHeader.SetTitle(LayoutElement.name);
            BlockHeader.SetParent(this);

            if (Animation != null)
            {
                BlockAnimation = new NodeBlockAnimation(layout, this);
                BlockAnimation.SetAnimation(LayoutElement, Animation);
                BlockAnimation.SetParent(this);
                BlockAnimation.Y = BlockHeader.Y + BlockHeader.Height + 10;
            }

            string[] outputs = UtilityReflection.GetMembersWithAttribute(layoutElement, typeof(UiLayoutProcessorHandler));

            for (int i = 0; i < outputs.Length; i++)
            {
                UiLayoutProcessorHandler a = UtilityReflection.GetAttribute<UiLayoutProcessorHandler>(layoutElement, outputs[i]);
                NodeBlockGroup blockGroup = GetGroup(a.Group);

                blockGroup.AddHandler(LayoutElement.PersistantID.Value, outputs[i], a.Weight);
            }

            string[] inputs = UtilityReflection.GetMembersWithAttribute(layoutElement, typeof(UiLayoutProcessorEvent));

            for (int i = 0; i < inputs.Length; i++)
            {
                UiLayoutProcessorEvent a = UtilityReflection.GetAttribute<UiLayoutProcessorEvent>(layoutElement, inputs[i]);
                NodeBlockGroup blockGroup = GetGroup(a.Group);

                blockGroup.AddSender(LayoutElement.PersistantID.Value, inputs[i], a.Weight);
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

            if (BlockAnimation != null)
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
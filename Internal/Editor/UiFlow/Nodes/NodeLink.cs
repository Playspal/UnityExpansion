using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeLink
    {
        public InternalUiFlowEditor FlowEditor { get; private set; }

        public readonly Node NodeA;
        public readonly Node NodeB;

        private NodeLinkIcon _iconA;
        private NodeLinkIcon _iconB;

        public NodeLink(EditorLayout layout, Node a, Node b)
        {
            FlowEditor = layout as InternalUiFlowEditor;

            NodeA = a;
            NodeB = b;

            _iconA = new NodeLinkIcon(layout);
            _iconA.Y = NodeA.Height - _iconA.Height / 2;
            _iconA.SetParent(NodeA);
            _iconA.SetAsFirstSibling();

            _iconB = new NodeLinkIcon(layout);
            _iconB.X = NodeB.Width / 2 - _iconB.Width / 2;
            _iconB.Y = -_iconB.Height / 2;
            _iconB.SetParent(NodeB);
            _iconB.SetAsFirstSibling();
        }

        public void SetPosition(int index, int total)
        {
            int width = total * _iconA.Width + (total - 1) * 10;
            int x = NodeA.Width / 2 - width / 2;

            _iconA.X = x + (_iconA.Width + 10) * index;
        }

        public void Render()
        {
            FlowEditor.Curves.AddToBackground
            (
                InternalUiFlowEditorCurve.Type.Vertical,
                _iconA.GetPositionGlobalX() + _iconA.Width / 2 - 1,
                _iconA.GetPositionGlobalY() + 14,
                _iconB.GetPositionGlobalX() + _iconB.Width / 2 + 1,
                _iconB.GetPositionGlobalY(),
                4,
                Color.white.Parse(InternalUiFlowEditorConfig.COLOR_NODE_BACKGROUND_BORDER)
            );

            FlowEditor.Curves.AddToBackground
            (
                InternalUiFlowEditorCurve.Type.Vertical,
                _iconA.GetPositionGlobalX() + _iconA.Width / 2 + 1,
                _iconA.GetPositionGlobalY() + 14,
                _iconB.GetPositionGlobalX() + _iconB.Width / 2 - 1,
                _iconB.GetPositionGlobalY(),
                4,
                Color.white.Parse(InternalUiFlowEditorConfig.COLOR_NODE_BACKGROUND_BORDER)
            );
        }
    }
}
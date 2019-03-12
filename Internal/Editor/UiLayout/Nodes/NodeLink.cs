using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLink
    {
        public UiLayoutEditor FlowEditor { get; private set; }

        public readonly Node NodeA;
        public readonly Node NodeB;

        private NodeLinkIcon _iconA;
        private NodeLinkIcon _iconB;

        private Color _color;

        public NodeLink(EditorLayout layout, Node a, Node b)
        {
            FlowEditor = layout as UiLayoutEditor;

            NodeA = a;
            NodeB = b;

            _iconA = new NodeLinkIcon(layout);
            _iconA.Y = NodeA.Height - _iconA.Height / 2;
            _iconA.SetParent(NodeA);
            _iconA.SetAsFirstSibling();

            _iconB = new NodeLinkIcon(layout);
            _iconB.X = NodeB.Width / 2 - _iconB.Width / 2;
            _iconB.Y = -_iconB.Height / 2 - 1;
            _iconB.SetParent(NodeB);
            _iconB.SetAsFirstSibling();

            _color = UiLayoutEditorConfig.ColorNodeBorder;
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
                UiLayoutEditorCurve.Type.Vertical,
                _iconA.GlobalX + _iconA.Width / 2,
                _iconA.GlobalY + 14,
                _iconB.GlobalX + _iconB.Width / 2,
                _iconB.GlobalY + 1,
                4,
                _color
            );

            FlowEditor.Curves.AddToBackground
            (
                UiLayoutEditorCurve.Type.Vertical,
                _iconA.GlobalX + _iconA.Width / 2 - 1,
                _iconA.GlobalY + 14,
                _iconB.GlobalX + _iconB.Width / 2 + 1,
                _iconB.GlobalY + 1,
                2,
                _color
            );

            FlowEditor.Curves.AddToBackground
            (
                UiLayoutEditorCurve.Type.Vertical,
                _iconA.GlobalX + _iconA.Width / 2 + 1,
                _iconA.GlobalY + 14,
                _iconB.GlobalX + _iconB.Width / 2 - 1,
                _iconB.GlobalY + 1,
                2,
                _color
            );
        }
    }
}
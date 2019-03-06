using System;

using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeConnector : EditorLayoutObject
    {
        public enum Type
        {
            Handler,
            Sender
        }

        public Action<NodeConnector> OnConnected;
        public Action<NodeConnector> OnDisconnected;

        public readonly Node Node;
        public readonly Type ConnectorType;

        public string DataID { get; private set; }
        public string DataMethod { get; private set; }

        public int Weight { get; private set; }

        public NodeConnector Connected { get; private set; }

        public NodeConnectorIcon Icon { get; private set; }
        public EditorLayoutObjectText Label { get; private set; }

        private const int HEIGHT = 15;

        public NodeConnector(EditorLayout layout, Node node, Type type, string label) : base(layout, node.Width, HEIGHT)
        {
            Node = node;
            ConnectorType = type;

            Weight = 0;

            SetupIcon();
            SetupLabel(label);
        }

        protected void SetupIcon()
        {
            Icon = new NodeConnectorIcon(Layout, Node, this);
            Icon.SetParent(this);
            Icon.X = ConnectorType == Type.Handler ? -Icon.Width / 2 : Width - Icon.Width / 2 - 1;
            Icon.Y = 0;
        }

        protected void SetupLabel(string text)
        {
            Label = new EditorLayoutObjectText(Layout, Width - 20, Height);
            Label.SetAlignment(ConnectorType == Type.Handler ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);
            Label.SetColor(UiLayoutEditorConfig.COLOR_NODE_LABEL);
            Label.SetText(text);
            Label.SetParent(this);
            Label.X = 10;
            Label.Y = -1;
        }

        public void SetWeight(int value)
        {
            Weight = value;
        }

        public void SetData(string id, string method)
        {
            DataID = id;
            DataMethod = method;
        }

        public override void Render()
        {
            base.Render();
        }

        public static void ConnectionCreate(NodeConnector a, NodeConnector b)
        {
            if (a.Connected != null)
            {
                ConnectionRemove(a, a.Connected);
            }

            if (b.Connected != null)
            {
                ConnectionRemove(b, b.Connected);
            }

            NodeConnectorHandler handler = (a.ConnectorType == Type.Handler ? a : b) as NodeConnectorHandler;
            NodeConnectorSender sender = (a.ConnectorType == Type.Sender ? a : b) as NodeConnectorSender;

            Color color = sender.Node.ColorMain;

            handler.Connected = sender;
            handler.Icon.SetColor(color);
            handler.OnConnected.InvokeIfNotNull(sender);

            sender.Connected = handler;
            sender.Icon.SetColor(color);
            sender.OnConnected.InvokeIfNotNull(handler);

            UiLayoutEditor.Instance.Selection.ProcessorEdictCreate
            (
                sender.DataID,
                sender.DataMethod,
                handler.DataID,
                handler.DataMethod
            );
        }

        public static void ConnectionRemove(NodeConnector a, NodeConnector b)
        {
            NodeConnectorHandler handler = (a.ConnectorType == Type.Handler ? a : b) as NodeConnectorHandler;
            NodeConnectorSender sender = (a.ConnectorType == Type.Sender ? a : b) as NodeConnectorSender;

            a.OnDisconnected.InvokeIfNotNull(a.Connected);
            a.Connected = null;
            a.Icon.ResetColor();

            b.OnDisconnected.InvokeIfNotNull(b.Connected);
            b.Connected = null;
            b.Icon.ResetColor();

            UiLayoutEditor.Instance.Selection.ProcessorEdictRemove
            (
                sender.DataID,
                sender.DataMethod,
                handler.DataID,
                handler.DataMethod
            );
        }
    }
}
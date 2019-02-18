using System;

using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeConnector : EditorLayoutObject
    {
        public enum Type
        {
            Input,
            Output
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
            Icon.X = ConnectorType == Type.Input ? -Icon.Width / 2 : Width - Icon.Width / 2 - 1;
            Icon.Y = 0;
        }

        protected void SetupLabel(string text)
        {
            Label = new EditorLayoutObjectText(Layout, Width - 20, Height);
            Label.SetAlignment(ConnectorType == Type.Input ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);
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

            NodeConnectorInput input = (a.ConnectorType == Type.Input ? a : b) as NodeConnectorInput;
            NodeConnectorOutput output = (a.ConnectorType == Type.Output ? a : b) as NodeConnectorOutput;

            Color color = output.Node.ColorMain;

            input.Connected = output;
            input.Icon.SetColor(color);
            input.OnConnected.InvokeIfNotNull(output);

            output.Connected = input;
            output.Icon.SetColor(color);
            output.OnConnected.InvokeIfNotNull(input);

            UiLayoutEditor.Instance.Selection.Target.ActionAdd(output.DataID, output.DataMethod, input.DataID, input.DataMethod);
        }

        public static void ConnectionRemove(NodeConnector a, NodeConnector b)
        {
            NodeConnectorInput input = (a.ConnectorType == Type.Input ? a : b) as NodeConnectorInput;
            NodeConnectorOutput output = (a.ConnectorType == Type.Output ? a : b) as NodeConnectorOutput;

            a.OnDisconnected.InvokeIfNotNull(a.Connected);
            a.Connected = null;
            a.Icon.ResetColor();

            b.OnDisconnected.InvokeIfNotNull(b.Connected);
            b.Connected = null;
            b.Icon.ResetColor();

            UiLayoutEditor.Instance.Selection.Target.ActionRemove(output.DataID, output.DataMethod, input.DataID, input.DataMethod);
        }
    }
}
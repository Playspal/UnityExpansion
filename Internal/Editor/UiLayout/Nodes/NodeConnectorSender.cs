using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeConnectorSender : NodeConnector
    {
        public NodeConnectorSender(EditorLayout layout, Node node, string label) : base(layout, node, Type.Sender, label)
        {
            node.Senders.Add(this);
        }
    }
}
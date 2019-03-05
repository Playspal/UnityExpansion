using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeConnectorOutput : NodeConnector
    {
        public NodeConnectorOutput(EditorLayout layout, Node node, string label) : base(layout, node, Type.Sender, label)
        {
            node.Output.Add(this);
        }
    }
}
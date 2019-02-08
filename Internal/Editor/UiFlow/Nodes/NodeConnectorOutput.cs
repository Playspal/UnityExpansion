using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeConnectorOutput : NodeConnector
    {
        public NodeConnectorOutput(EditorLayout layout, Node node, string label) : base(layout, node, Type.Output, label)
        {
        }
    }
}
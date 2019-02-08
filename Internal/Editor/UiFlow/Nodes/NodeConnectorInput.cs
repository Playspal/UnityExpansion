using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeConnectorInput : NodeConnector
    {
        public NodeConnectorInput(EditorLayout layout, Node node, string label) : base(layout, node, Type.Input, label)
        {
        }
    }
}
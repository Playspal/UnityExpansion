using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeConnectorInput : NodeConnector
    {
        public NodeConnectorInput(EditorLayout layout, Node node, string label) : base(layout, node, Type.Handler, label)
        {
            node.Input.Add(this);
        }
    }
}
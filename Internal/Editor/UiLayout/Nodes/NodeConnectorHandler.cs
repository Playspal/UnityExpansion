using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeConnectorHandler : NodeConnector
    {
        public NodeConnectorHandler(EditorLayout layout, Node node, string label) : base(layout, node, Type.Handler, label)
        {
            node.Handlers.Add(this);
        }
    }
}
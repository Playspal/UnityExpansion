using UnityExpansion.Editor;
using UnityExpansion.UI.Layout;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeSystemMethod : NodeSystem
    {
        public readonly NodeConnectorInput ConnectorInput;

        public NodeSystemMethod(InternalUiLayoutData.NodeData nodeData, EditorLayout layout) : base(nodeData, layout)
        {
            ConnectorInput = new NodeConnectorInput(layout, this, string.Empty);
            ConnectorInput.SetParent(this);
            ConnectorInput.Y = Height / 2 - 6;

            SetConnector(ConnectorInput);
        }

        public void SetData(UiLayout uiLayout, string methodName)
        {
            ConnectorInput.SetData(uiLayout.PersistantID.Value, methodName);
            SetTitle(methodName);
        }
    }
}
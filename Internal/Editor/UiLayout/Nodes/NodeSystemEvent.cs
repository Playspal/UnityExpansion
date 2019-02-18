using UnityExpansion.Editor;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeSystemEvent : NodeSystem
    {
        public readonly NodeConnectorOutput ConnectorOutput;

        public NodeSystemEvent(InternalUiLayoutData.NodeData nodeData, EditorLayout layout) : base(nodeData, layout)
        {
            ConnectorOutput = new NodeConnectorOutput(layout, this, string.Empty);
            ConnectorOutput.SetParent(this);
            ConnectorOutput.Y = Height / 2 - 6;

            SetConnector(ConnectorOutput);
        }

        public void SetData(UiLayout uiLayout, string eventName)
        {
            ConnectorOutput.SetData(uiLayout.UniqueID, eventName);
            SetTitle(eventName);
        }
    }
}
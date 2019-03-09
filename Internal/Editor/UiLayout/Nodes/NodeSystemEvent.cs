using UnityExpansion.Editor;
using UnityExpansion.UI.Layout;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeSystemEvent : NodeSystem
    {
        public readonly NodeConnectorSender Sender;

        public NodeSystemEvent(InternalUiLayoutData.NodeData nodeData, EditorLayout layout) : base(nodeData, layout)
        {
            Sender = new NodeConnectorSender(layout, this, string.Empty);
            Sender.SetParent(this);
            Sender.Y = Height / 2 - 6;

            SetConnector(Sender);
        }

        public void SetData(UiLayout uiLayout, string eventName)
        {
            Sender.SetData(uiLayout.PersistantID.Value, eventName);
            SetTitle(eventName);
        }
    }
}
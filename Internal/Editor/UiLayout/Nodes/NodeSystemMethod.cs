using UnityExpansion.Editor;
using UnityExpansion.UI.Layout;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeSystemMethod : NodeSystem
    {
        public readonly NodeConnectorHandler Handler;

        public NodeSystemMethod(InternalUiLayoutData.NodeData nodeData, EditorLayout layout) : base(nodeData, layout)
        {
            Handler = new NodeConnectorHandler(layout, this, string.Empty);
            Handler.SetParent(this);
            Handler.Y = Height / 2 - 6;

            SetConnector(Handler);
        }

        public void SetData(UiLayout uiLayout, string methodName)
        {
            Handler.SetData(uiLayout.PersistantID.Value, methodName);
            SetTitle(methodName);
        }
    }
}
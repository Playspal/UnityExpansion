using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLayoutElementRoot : NodeLayoutElement
    {
        public NodeLayoutElementRoot(InternalUiLayoutData.NodeData nodeData, EditorLayout layout) : base(nodeData, layout)
        {
            BlockShowAndHide.InputShow.Label.SetText("Instantiate and Show");
            BlockShowAndHide.InputHide.Label.SetText("Hide and Destroy");
        }
    }
}
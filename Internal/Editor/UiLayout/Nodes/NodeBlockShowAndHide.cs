using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeBlockShowAndHide : EditorLayoutObject
    {
        public readonly NodeConnectorInput InputShow;
        public readonly NodeConnectorInput InputHide;

        public readonly NodeConnectorOutput OutputOnShow;
        public readonly NodeConnectorOutput OutputOnHide;

        public NodeBlockShowAndHide(EditorLayout layout, Node node) : base(layout, node.Width, 40)
        {
            InputShow = new NodeConnectorInput(layout, node, "Show");
            InputShow.SetParent(this);

            InputHide = new NodeConnectorInput(layout, node, "Hide");
            InputHide.SetParent(this);
            InputHide.Y = 20;

            OutputOnShow = new NodeConnectorOutput(layout, node, "OnShow");
            OutputOnShow.SetParent(this);

            OutputOnHide = new NodeConnectorOutput(layout, node, "OnHide");
            OutputOnHide.SetParent(this);
            OutputOnHide.Y = 20;
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
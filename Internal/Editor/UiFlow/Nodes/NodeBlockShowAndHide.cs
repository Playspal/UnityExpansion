using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeBlockShowAndHide : EditorLayoutObject
    {
        public readonly NodeConnectorInput InputShow;
        public readonly NodeConnectorInput InputHide;

        public readonly NodeConnectorOutput OutputOnShow;
        public readonly NodeConnectorOutput OutputOnHide;

        public NodeBlockShowAndHide(EditorLayout layout, Node node) : base(layout, node.Width, 30)
        {
            InputShow = new NodeConnectorInput(layout, node, "Show");
            InputShow.SetParent(this);
            InputShow.Y = 10;

            InputHide = new NodeConnectorInput(layout, node, "Hide");
            InputHide.SetParent(this);
            InputHide.Y = 30;

            OutputOnShow = new NodeConnectorOutput(layout, node, "OnShow");
            OutputOnShow.SetParent(this);
            OutputOnShow.Y = 10;

            OutputOnHide = new NodeConnectorOutput(layout, node, "OnHide");
            OutputOnHide.SetParent(this);
            OutputOnHide.Y = 30;
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
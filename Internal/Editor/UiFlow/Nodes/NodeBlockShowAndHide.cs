using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeBlockShowAndHide : EditorLayoutObject
    {
        private NodeConnectorInput _inputShow;
        private NodeConnectorInput _inputHide;

        private NodeConnectorOutput _outputOnShow;
        private NodeConnectorOutput _outputOnHide;

        public NodeBlockShowAndHide(EditorLayout layout, Node node) : base(layout, node.Width, 30)
        {
            _inputShow = new NodeConnectorInput(layout, Width / 2, "Show");
            _inputShow.SetParent(this);
            _inputShow.Y = 10;

            _inputHide = new NodeConnectorInput(layout, Width / 2, "Hide");
            _inputHide.SetParent(this);
            _inputHide.Y = 30;

            _outputOnShow = new NodeConnectorOutput(layout, Width / 2, "OnShow");
            _outputOnShow.SetParent(this);
            _outputOnShow.X = Width / 2;
            _outputOnShow.Y = 10;

            _outputOnHide = new NodeConnectorOutput(layout, Width / 2, "OnHide");
            _outputOnHide.SetParent(this);
            _outputOnHide.X = Width / 2;
            _outputOnHide.Y = 30;
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
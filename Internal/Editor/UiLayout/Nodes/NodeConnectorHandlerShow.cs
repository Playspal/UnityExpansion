using UnityEngine;
using UnityExpansion.Editor;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeConnectorHandlerShow : NodeConnectorHandler
    {
        private readonly NodeLayoutElement _node;
        private EditorLayoutObjectText _label;

        public NodeConnectorHandlerShow(EditorLayout layout, NodeLayoutElement node) : base(layout, node, "Show..")
        {
            _node = node;

            if (_node.Animation != null)
            {
                UiAnimationClip clip = _node.Animation.GetAnimationClip(_node.LayoutElement.AnimationShowID);
                string animationName = clip != null ? "With " + clip.Name : "Without animation";

                _label = new EditorLayoutObjectText(Layout, node.Width - 100, 20);
                _label.SetAlignment(TextAnchor.MiddleLeft);
                _label.SetColor(node.ColorMain);
                _label.SetText(animationName);
                _label.SetFontStyle(FontStyle.Italic);
                _label.SetParent(this);
                _label.X = 60;
                _label.Y = -2;
            }
        }
    }
}
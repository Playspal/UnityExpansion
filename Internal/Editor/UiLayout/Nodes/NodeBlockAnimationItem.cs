using UnityEngine;
using UnityExpansion.Editor;
using UnityExpansion.UI;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeBlockAnimationItem : EditorLayoutObject
    {
        public readonly NodeConnectorInput InputPlay;

        public readonly NodeConnectorOutput OutputOnComplete;

        private EditorLayoutObjectText _label;

        public NodeBlockAnimationItem(EditorLayout layout, Node node, UiAnimationClip clip) : base(layout, node.Width, 20)
        {
            InputPlay = new NodeConnectorInput(layout, node, "Play");
            InputPlay.SetData(clip.ID, UiLayout.PREDEFINED_METHOD_ANIMATION_PLAY);
            InputPlay.SetParent(this);
            InputPlay.Y = 0;

            OutputOnComplete = new NodeConnectorOutput(layout, node, "OnComplete");
            OutputOnComplete.SetData(clip.ID, UiLayout.PREDEFINED_EVENT_ANIMATION_ON_COMPLETE);
            OutputOnComplete.SetParent(this);
            OutputOnComplete.Y = 0;

            _label = new EditorLayoutObjectText(Layout, node.Width - 100, 20);
            _label.SetAlignment(TextAnchor.MiddleLeft);
            _label.SetColor(node.ColorMain);
            _label.SetText(clip.Name);
            _label.SetFontStyle(FontStyle.Italic);
            _label.SetParent(this);
            _label.X = 40;
            _label.Y = -2;
        }
    }
}
using UnityEngine;
using UnityExpansion.Editor;

using UnityExpansion.UI.Animation;
using UnityExpansion.UI.Layout;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeBlockAnimationItem : EditorLayoutObject
    {
        public readonly NodeConnectorInput InputPlay;

        public readonly NodeConnectorOutput OutputOnComplete;

        private EditorLayoutObjectText _label;

        public NodeBlockAnimationItem(EditorLayout layout, Node node, UiLayoutElement layoutElement, UiAnimationClip clip) : base(layout, node.Width, 20)
        {
            InputPlay = new NodeConnectorInput(layout, node, UiLayoutEditorConfig.PREDEFINED_METHOD_ANIMATION_PLAY);
            InputPlay.SetData(clip.ID.ToString(), UiLayoutEditorConfig.PREDEFINED_METHOD_ANIMATION_PLAY);
            InputPlay.SetParent(this);
            InputPlay.Y = 0;

            OutputOnComplete = new NodeConnectorOutput(layout, node, UiLayoutEditorConfig.PREDEFINED_EVENT_ANIMATION_ON_COMPLETE);
            OutputOnComplete.SetData(clip.ID.ToString(), UiLayoutEditorConfig.PREDEFINED_EVENT_ANIMATION_ON_COMPLETE);
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
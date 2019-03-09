using UnityEngine;
using UnityExpansion.Editor;

using UnityExpansion.UI.Animation;
using UnityExpansion.UI.Layout;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeBlockAnimationItem : EditorLayoutObject
    {
        public readonly NodeConnectorHandler Handler;

        public readonly NodeConnectorSender Sender;

        private EditorLayoutObjectText _label;

        public NodeBlockAnimationItem(EditorLayout layout, Node node, UiLayoutElement layoutElement, UiAnimationClip clip) : base(layout, node.Width, 20)
        {
            Handler = new NodeConnectorHandler(layout, node, UiLayoutEditorConfig.PREDEFINED_METHOD_ANIMATION_PLAY);
            Handler.SetData(clip.ID.ToString(), UiLayoutEditorConfig.PREDEFINED_METHOD_ANIMATION_PLAY);
            Handler.SetParent(this);
            Handler.Y = 0;

            Sender = new NodeConnectorSender(layout, node, UiLayoutEditorConfig.PREDEFINED_EVENT_ANIMATION_ON_COMPLETE);
            Sender.SetData(clip.ID.ToString(), UiLayoutEditorConfig.PREDEFINED_EVENT_ANIMATION_ON_COMPLETE);
            Sender.SetParent(this);
            Sender.Y = 0;

            _label = new EditorLayoutObjectText(Layout, node.Width - 100, 20);
            _label.SetAlignment(TextAnchor.MiddleLeft);
            _label.SetColor(UiLayoutEditorConfig.ColorNodeLabelSpecial);
            _label.SetText(clip.Name);
            _label.SetFontStyle(FontStyle.Italic);
            _label.SetParent(this);
            _label.X = 40;
            _label.Y = -3;

            if(layoutElement.AnimationShowID == clip.ID || layoutElement.AnimationHideID == clip.ID)
            {
                Handler.SetActive(false);

                _label.X = 10;
            }
        }
    }
}
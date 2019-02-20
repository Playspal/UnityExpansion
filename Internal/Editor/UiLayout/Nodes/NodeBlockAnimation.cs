using System.Collections.Generic;
using UnityExpansion.Editor;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeBlockAnimation : EditorLayoutObject
    {
        public readonly Node Node;

        private EditorLayoutObjectText _title;
        private EditorLayoutObjectTexture _hline;
        private List<NodeBlockAnimationItem> _items = new List<NodeBlockAnimationItem>();

        public NodeBlockAnimation(EditorLayout layout, Node node) : base(layout, node.Width, 30)
        {
            Node = node;

            _hline = new EditorLayoutObjectTexture(layout, Width, 1);
            _hline.Fill(node.ColorBackgroundBorder);
            _hline.SetParent(this);

            _title = new EditorLayoutObjectText(layout, Width, 20);
            _title.SetAlignment(UnityEngine.TextAnchor.MiddleLeft);
            _title.SetFontStyle(UnityEngine.FontStyle.Bold);
            _title.SetColor(UiLayoutEditorConfig.COLOR_NODE_LABEL);
            _title.SetText("Attached animations");
            _title.SetParent(this);
            _title.Y = _hline.Height + 5;
            _title.X = 9;
        }

        public void SetAnimation(UiAnimation uiAnimation)
        {
            for(int i = 0; i < uiAnimation.AnimationClips.Count; i++)
            {
                AddAnimation(uiAnimation.AnimationClips[i]);
            }

            if (_items.Count > 0)
            {
                SetSize(Width, _items.Count * _items[0].Height + _title.Y + _title.Height + 5);
            }
        }

        private void AddAnimation(UiAnimationClip clip)
        {
            NodeBlockAnimationItem item = new NodeBlockAnimationItem(Layout, Node, clip);
            item.SetParent(this);
            item.Y = _items.Count * item.Height + _title.Y + _title.Height + 5;

            _items.Add(item);
        }
    }
}
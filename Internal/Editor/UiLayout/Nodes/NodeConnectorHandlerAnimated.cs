using System.Collections.Generic;

using UnityExpansion;
using UnityExpansion.Editor;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeConnectorHandlerAnimated : NodeConnectorHandler
    {
        public enum AnimationType
        {
            Show,
            Hide
        }

        private const string NO_ANIMATION = "No animation";

        private readonly NodeLayoutElement _node;
        private readonly UiLayoutEditorDropDownButton _button;
        private readonly AnimationType _animationType;

        public NodeConnectorHandlerAnimated(EditorLayout layout, NodeLayoutElement node, AnimationType animationType) : base(layout, node, string.Empty)
        {
            _node = node;
            _animationType = animationType;

            switch(_animationType)
            {
                case AnimationType.Show:
                    Label.SetText("Show");
                    break;

                case AnimationType.Hide:
                    Label.SetText("Hide");
                    break;
            }

            if (_node.Animation != null)
            {
                UiAnimationClip clip = GetSelectedClip();
                string animationName = clip != null ? clip.Name : NO_ANIMATION;

                _button = new UiLayoutEditorDropDownButton(Layout, Node.Width - 12, 18);
                _button.OnClick += OnButtonClick;
                _button.SetParent(this);
                _button.SetLabel(animationName);
                _button.X = 6;
                _button.Y = -2;
            }
        }

        private void OnButtonClick()
        {
            // Prepare data
            List<CommonPair<string, object>> data = new List<CommonPair<string, object>>();

            data.Add(new CommonPair<string, object>(NO_ANIMATION, null));

            for (int i = 0; i < _node.Animation.AnimationClips.Count; i++)
            {
                UiAnimationClip clip = _node.Animation.AnimationClips[i];

                data.Add
                (
                    new CommonPair<string, object>(clip.Name, clip)
                );
            }

            // Show dropdown list
            UiLayoutEditorDropDownList list = new UiLayoutEditorDropDownList(Layout);

            list.SetData(data);
            list.X = _button.GlobalX + _button.Width - list.Width - Layout.CanvasX;
            list.Y = _button.GlobalY - Layout.CanvasY;
            list.OnSelect += (object selected) =>
            {
                SetSelectedClip(selected as UiAnimationClip);
                (Layout as UiLayoutEditor).RefreshNode(Node);
            };
        }

        private void SetSelectedClip(UiAnimationClip clip)
        {
            string id = clip != null ? clip.ID.ToString() : string.Empty;

            switch (_animationType)
            {
                case AnimationType.Show:
                    if(_node.LayoutElement.AnimationHideID == id)
                    {
                        _node.LayoutElement.AnimationHideID = string.Empty;
                    }

                    _node.LayoutElement.AnimationShowID = id;
                    break;

                case AnimationType.Hide:
                    if (_node.LayoutElement.AnimationShowID == id)
                    {
                        _node.LayoutElement.AnimationShowID = string.Empty;
                    }

                    _node.LayoutElement.AnimationHideID = id;
                    break;
            }
        }

        private UiAnimationClip GetSelectedClip()
        {
            switch (_animationType)
            {
                case AnimationType.Show:
                    return _node.Animation.GetAnimationClip(_node.LayoutElement.AnimationShowID);

                case AnimationType.Hide:
                    return _node.Animation.GetAnimationClip(_node.LayoutElement.AnimationHideID);
            }

            return null;
        }
    }
}
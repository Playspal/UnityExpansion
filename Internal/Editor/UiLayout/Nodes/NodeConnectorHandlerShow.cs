using System.Collections.Generic;
using UnityEngine;
using UnityExpansion;
using UnityExpansion.Editor;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeConnectorHandlerShow : NodeConnectorHandler
    {
        private readonly NodeLayoutElement _node;
        private readonly UiLayoutEditorDropDownButton _button;

        public NodeConnectorHandlerShow(EditorLayout layout, NodeLayoutElement node) : base(layout, node, "Show")
        {
            _node = node;

            if (_node.Animation != null)
            {
                UiAnimationClip clip = _node.Animation.GetAnimationClip(_node.LayoutElement.AnimationShowID);
                string animationName = clip != null ? "With " + clip.Name : "Without animation ⯆ a";

                _button = new UiLayoutEditorDropDownButton(Layout, Node.Width - 12, 18);
                _button.OnClick += OnButtonClick;
                _button.SetParent(this);
                _button.SetColor(node.ColorMain);
                _button.X = 6;
                _button.Y = -2;
            }
        }

        private void OnButtonClick()
        {
            // Prepare data
            List<CommonPair<string, object>> data = new List<CommonPair<string, object>>();
            for(int i = 0; i < _node.Animation.AnimationClips.Count; i++)
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
            list.X = _button.GlobalX + _button.Width - list.Width;
            list.Y = _button.GlobalY;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeLayoutElement : Node
    {
        private NodeBlockHeader _header;
        private NodeBlockShowAndHide _blockShowAndHide;

        public UiLayoutElement LayoutElement { get; private set; }

        public NodeLayoutElement(EditorLayout layout) : base(layout, 300, 200)
        {
            _header = new NodeBlockHeader(layout, this);
            _header.SetParent(this);

            _blockShowAndHide = new NodeBlockShowAndHide(layout, this);
            _blockShowAndHide.SetParent(this);
            _blockShowAndHide.Y = 30;

            Layout.Mouse.OnPress += MouseHandlerPress;
            Layout.Mouse.OnRelease += MouseHandlerRelease;
        }

        public override void Destroy()
        {
            base.Destroy();

            Layout.Mouse.OnPress -= MouseHandlerPress;
            Layout.Mouse.OnRelease -= MouseHandlerRelease;
        }

        public void SetLayoutElement(UiLayoutElement layoutElement)
        {
            LayoutElement = layoutElement;

            _header.SetTitle(LayoutElement.name);
        }

        private void MouseHandlerPress()
        {
            if (_header.HitTest(Layout.Mouse.X, Layout.Mouse.Y))
            {
                DragStart(false);
            }
        }

        private void MouseHandlerRelease()
        {
            if (IsDragging)
            {
                DragStop();
            }
        }

        private void MouseHandlerClick() { }
    }
}
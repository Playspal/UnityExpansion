using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeLayoutEvent : Node
    {
        public enum Type
        {
            OnEnable
        }

        public readonly Type EventType;
        public readonly NodeConnectorOutput OutputOnEvent;

        public UiLayout UiLayout { get; private set; }

        private EditorLayoutObjectTexture _textureHeader;
        private EditorLayoutObjectText _title;

        public NodeLayoutEvent(EditorLayout layout, Type eventType) : base(layout, 200, 50)
        {
            EventType = eventType;

            OutputOnEvent = new NodeConnectorOutput(layout, this, string.Empty);
            OutputOnEvent.SetParent(this);
            OutputOnEvent.Y = Height / 2 - 6;

            OutputOnEvent.OnConnected += (NodeConnector nodeConnector) =>
            {
                if (UiLayout != null)
                {
                    UiLayout.SignalsOnEnable = UiLayoutEditorUtils.SignalsAdd(UiLayout.SignalsOnEnable, nodeConnector.Data);
                }
            };

            OutputOnEvent.OnDisconnected += (NodeConnector nodeConnector) =>
            {
                if (UiLayout != null)
                {
                    UiLayout.SignalsOnEnable = UiLayoutEditorUtils.SignalsRemove(UiLayout.SignalsOnEnable, nodeConnector.Data);
                }
            };

            _textureBackground = new EditorLayoutObjectTexture(layout, Width - 2, 6);
            _textureBackground.X = _textureBackground.Y = 1;
            _textureBackground.Fill(ColorMain);
            _textureBackground.DrawBorderBottom(1, ColorDark);
            _textureBackground.SetParent(this);

            _title = new EditorLayoutObjectText(layout, Width, Height - 6);
            _title.SetAlignment(TextAnchor.MiddleCenter);
            _title.SetFontStyle(FontStyle.Bold);
            _title.SetColor(ColorMain);
            _title.SetText("ON ENABLE");
            _title.SetParent(this);
            _title.Y = 5;

            Layout.Mouse.OnPress += MouseHandlerPress;
            Layout.Mouse.OnRelease += MouseHandlerRelease;
        }

        public override void Destroy()
        {
            base.Destroy();

            Layout.Mouse.OnPress -= MouseHandlerPress;
            Layout.Mouse.OnRelease -= MouseHandlerRelease;
        }

        public void SetUiLayout(UiLayout uiLayout)
        {
            UiLayout = uiLayout;
        }


        private void MouseHandlerPress()
        {
            if (HitTest(Layout.Mouse.X, Layout.Mouse.Y) && !OutputOnEvent.Icon.HitTest(Layout.Mouse.X, Layout.Mouse.Y))
            {
                DragStart(false);
            }
        }

        private void MouseHandlerRelease()
        {
            if (IsDragging)
            {
                DragStop();

                X = Mathf.RoundToInt((float)X / 20f) * 20;
                Y = Mathf.RoundToInt((float)Y / 20f) * 20;
            }
        }
    }
}
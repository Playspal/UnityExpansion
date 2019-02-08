using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeConnectorIcon : EditorLayoutObject
    {
        public readonly NodeConnector Connector;

        public Color ColorConnected { get; private set; }
        public Color ColorCurrent { get; private set; }

        protected EditorLayoutObjectTexture _texture;

        private bool _curveDragging = false;

        public NodeConnectorIcon(EditorLayout layout, Node node, NodeConnector connector) : base(layout, 15, 15)
        {
            Connector = connector;

            ColorConnected = node.ColorMain;

            _texture = new EditorLayoutObjectTexture(layout, Width, Height);
            _texture.Fill(new Color(0, 0, 0, 0));
            _texture.DrawRhombus(7, 7, 9 , node.ColorBackground);
            _texture.SetParent(this);

            Layout.Mouse.OnPress += MouseHandlerPress;
            Layout.Mouse.OnRelease += MouseHandlerRelease;

            ResetColor();
        }

        public override void Destroy()
        {
            base.Destroy();

            Layout.Mouse.OnPress -= MouseHandlerPress;
            Layout.Mouse.OnRelease -= MouseHandlerRelease;
        }

        public void SetColor(Color color)
        {
            ColorCurrent = color;
            _texture.DrawRhombus(7, 7, 6, ColorCurrent);
        }

        public void ResetColor()
        {
            ColorCurrent = Color.white;
            _texture.DrawRhombus(7, 7, 6, ColorCurrent);
        }

        public void StartConnectionDrag()
        {
            _curveDragging = true;
        }

        public void StopConnectionDrag()
        {
            _curveDragging = false;
        }

        public override void Render()
        {
            base.Render();

            if(_curveDragging)
            {
                RenderCurveToMouse();
            }
            else if(Connector.ConnectorType == NodeConnector.Type.Output && Connector.Connected != null)
            {
                RenderCurveToConnection();
            }
        }

        private void RenderCurveToMouse()
        {
            RenderCurveTo
            (
                Layout.Mouse.X,
                Layout.Mouse.Y
            );
        }

        private void RenderCurveToConnection()
        {
            RenderCurveTo
            (
                Connector.Connected.Icon.GetPositionGlobalX() + Connector.Connected.Icon.Width / 2,
                Connector.Connected.Icon.GetPositionGlobalY() + Connector.Connected.Icon.Height / 2
            );
        }

        private void RenderCurveTo(int toX, int toY)
        {
            int fromX = GetPositionGlobalX() + Width / 2;
            int fromY = GetPositionGlobalY() + Height / 2;

            ((InternalUiFlowEditor)Layout).Curves.AddToFrontground(InternalUiFlowEditorCurve.Type.Horizontal, fromX, fromY, toX, toY, 3, ColorCurrent);
        }

        private void MouseHandlerPress()
        {
            if (HitTest(Layout.Mouse.X, Layout.Mouse.Y))
            {
                if(Connector.Connected != null)
                {
                    Connector.Connected.Icon.StartConnectionDrag();
                    NodeConnector.ConnectionRemove(Connector, Connector.Connected);
                }
                else
                {
                    StartConnectionDrag();
                }
            }
        }

        private void MouseHandlerRelease()
        {
            if (_curveDragging)
            {
                _curveDragging = false;

                List<NodeConnectorIcon> icons = Layout.Objects.FindAllObjects<NodeConnectorIcon>();

                for(int i = 0; i < icons.Count; i++)
                {
                    if (icons[i].HitTest(Layout.Mouse.X, Layout.Mouse.Y))
                    {
                        if (icons[i].Connector.ConnectorType != Connector.ConnectorType)
                        {
                            NodeConnector.ConnectionCreate(Connector, icons[i].Connector);

                            break;
                        }
                    }
                }
            }
        }
    }
}
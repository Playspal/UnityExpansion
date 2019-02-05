using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeConnector : EditorLayoutObject
    {
        public enum Type
        {
            Input,
            Output
        }

        public NodeConnector Connection { get; private set; }

        protected EditorLayoutTexture2D _textureBackground;

        private const string COLOR_BACKGROUND = "#3A3A3A";
        private const string COLOR_CENTER = "#FFFFFF";

        public Color ColorNormal { get; private set; }
        public Color ColorConnected { get; private set; }
        public Color ColorCurrent { get; private set; }

        public Type ConnectionType;

        private bool _curveDragging = false;

        public NodeConnector(EditorLayout layout, Node node, Type direction) : base(layout, 15, 15)
        {
            ConnectionType = direction;

            ColorNormal = Color.white;
            ColorConnected = node.ColorMain;

            _textureBackground = new EditorLayoutTexture2D(layout, Width, Height);
            _textureBackground.Fill(new Color(0, 0, 0, 0));
            _textureBackground.DrawRhombus(7, 7, Width, COLOR_BACKGROUND);
            _textureBackground.DrawRhombus(7, 7, 9, COLOR_CENTER);
            _textureBackground.SetParent(this);

            Layout.Mouse.OnPress += MouseHandlerPress;
            Layout.Mouse.OnRelease += MouseHandlerRelease;

            SetColor(ColorNormal);
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
            _textureBackground.DrawRhombus(7, 7, 9, ColorCurrent);
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
            else if(ConnectionType == Type.Output && Connection != null)
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
                Connection.GetPositionGlobalX() + Connection.Width / 2,
                Connection.GetPositionGlobalY() + Connection.Height / 2
            );
        }

        private void RenderCurveTo(int toX, int toY)
        {
            int fromX = GetPositionGlobalX() + Width / 2;
            int fromY = GetPositionGlobalY() + Height / 2;

            InternalUiFlowEditorCurve curve = null;

            curve = new InternalUiFlowEditorCurve(fromX, fromY, toX, toY);
            curve.SetStyle(3, ColorCurrent);

            ((InternalUiFlowEditor)Layout).Curves.AddToFrontground(curve);
        }

        private void MouseHandlerPress()
        {
            if (HitTest(Layout.Mouse.X, Layout.Mouse.Y))
            {
                if(Connection != null)
                {
                    Connection.StartConnectionDrag();
                    Disconnect(this, Connection);
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

                List<NodeConnector> nodeConnectors = Layout.Objects.FindAllObjects<NodeConnector>();

                for(int i = 0; i < nodeConnectors.Count; i++)
                {
                    if(nodeConnectors[i].ConnectionType != ConnectionType && nodeConnectors[i].HitTest(Layout.Mouse.X, Layout.Mouse.Y))
                    {
                        Connect(this, nodeConnectors[i]);

                        break;
                    }
                }
            }
        }

        public static void Connect(NodeConnector a, NodeConnector b)
        {
            if (a.Connection != null)
            {
                Disconnect(a, a.Connection);
            }

            if (b.Connection != null)
            {
                Disconnect(b, b.Connection);
            }

            Color color = a.ConnectionType == Type.Output ? a.ColorConnected : b.ColorConnected;

            a.Connection = b;
            a.SetColor(color);

            b.Connection = a;
            b.SetColor(color);
        }

        public static void Disconnect(NodeConnector a, NodeConnector b)
        {
            a.Connection = null;
            a.SetColor(a.ColorNormal);

            b.Connection = null;
            b.SetColor(a.ColorNormal);
        }
    }
}
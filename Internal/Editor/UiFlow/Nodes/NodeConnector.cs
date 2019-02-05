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

        public Type ConnectionType;

        private bool _curveDragging = false;

        public NodeConnector(EditorLayout layout, Type direction) : base(layout, 15, 15)
        {
            ConnectionType = direction;

            _textureBackground = new EditorLayoutTexture2D(Width, Height);
            _textureBackground.Fill(new Color(0, 0, 0, 0));
            _textureBackground.DrawRhombus(7, 7, Width, COLOR_BACKGROUND);
            _textureBackground.DrawRhombus(7, 7, 9, COLOR_CENTER);

            Layout.Mouse.OnPress += MouseHandlerPress;
            Layout.Mouse.OnRelease += MouseHandlerRelease;
        }

        public override void Destroy()
        {
            base.Destroy();

            Layout.Mouse.OnPress -= MouseHandlerPress;
            Layout.Mouse.OnRelease -= MouseHandlerRelease;
        }

        public void Connect(NodeConnector target)
        {
            Disconnect();
            Connection = target;
            target.Connection = this;
        }

        public void Disconnect()
        {
            if(Connection != null)
            {
                Connection.Connection = null;
                Connection = null;
            }
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

            _textureBackground.Render(GetPositionGlobalX(), GetPositionGlobalY());

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
            int centerX = GetPositionGlobalX() + Width / 2;
            int centerY = GetPositionGlobalY() + Height / 2;

            InternalUiFlowEditorCurve curve = null;

            curve = new InternalUiFlowEditorCurve(centerX, centerY, Layout.Mouse.X, Layout.Mouse.Y);
            curve.SetStyle(3, "#FFFFFF");

            ((InternalUiFlowEditor)Layout).Curves.AddToFrontground(curve);
        }

        private void RenderCurveToConnection()
        {
            int centerX = GetPositionGlobalX() + Width / 2;
            int centerY = GetPositionGlobalY() + Height / 2;

            int centerConnectionX = Connection.GetPositionGlobalX() + Connection.Width / 2;
            int centerConnectionY = Connection.GetPositionGlobalY() + Connection.Height / 2;

            InternalUiFlowEditorCurve curve = null;

            curve = new InternalUiFlowEditorCurve(centerX, centerY, centerConnectionX, centerConnectionY);
            curve.SetStyle(3, "#FFFFFF");

            ((InternalUiFlowEditor)Layout).Curves.AddToFrontground(curve);
        }

        private void MouseHandlerPress()
        {
            if (HitTest(Layout.Mouse.X, Layout.Mouse.Y))
            {
                if(Connection != null)
                {
                    Connection.StartConnectionDrag();
                    Disconnect();
                }
                else
                {
                    StartConnectionDrag();
                    Disconnect();
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
                        Connect(nodeConnectors[i]);

                        break;
                    }
                }
            }
        }
    }
}
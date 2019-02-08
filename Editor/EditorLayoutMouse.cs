using System;
using UnityEngine;

namespace UnityExpansion.Editor
{
    /// <summary>
    /// This class provides mouse input relativ to parent EditorLayout.
    /// Normally instance created and used by EditorLayout.
    /// </summary>
    public class EditorLayoutMouse
    {
        public event Action OnPress;
        public event Action OnRelease;
        public event Action OnClick;
        public event Action OnClickContext;
        public event Action OnMove;

        public event Action OnDragContext;

        public EditorLayout Layout { get; private set; }

        /// <summary>
        /// Mouse X.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Mouse Y.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Mouse X.
        /// </summary>
        public int GlobalX { get; private set; }

        /// <summary>
        /// Mouse Y.
        /// </summary>
        public int GlobalY { get; private set; }

        public int DeltaX { get; private set; }
        public int DeltaY { get; private set; }

        /// <summary>
        /// Is the left mouse button currently pressed.
        /// </summary>
        public bool IsPressed { get; private set; }

        public bool IsContextPressed { get; private set; }

        private float _x = 0;
        private float _y = 0;

        public EditorLayoutMouse(EditorLayout layout)
        {
            Layout = layout;
        }

        /// <summary>
        /// OnGui event parsing to update mouse state.
        /// </summary>
        public void OnGui()
        {
            switch(Event.current.type)
            {
                case EventType.Repaint:

                    DeltaX = Mathf.RoundToInt(Event.current.mousePosition.x - _x);
                    DeltaY = Mathf.RoundToInt(Event.current.mousePosition.y - _y);

                    _x = Event.current.mousePosition.x;
                    _y = Event.current.mousePosition.y;

                    X = Mathf.RoundToInt(Event.current.mousePosition.x);
                    Y = Mathf.RoundToInt(Event.current.mousePosition.y);

                    GlobalX = X + Layout.CanvasX;
                    GlobalY = Y + Layout.CanvasY;
                    break;

                case EventType.MouseDown:
                    if (Event.current.button == 0)
                    {
                        IsPressed = true;
                        OnPress.InvokeIfNotNull();
                    }

                    if (Event.current.button == 1)
                    {
                        IsContextPressed = true;
                    }
                    break;

                case EventType.MouseUp:
                    if (Event.current.button == 0)
                    {
                        IsPressed = false;
                        OnRelease.InvokeIfNotNull();
                        OnClick.InvokeIfNotNull();
                    }

                    if (Event.current.button == 1)
                    {
                        IsContextPressed = false;
                    }


                    break;

                case EventType.MouseMove:
                    OnMove.InvokeIfNotNull();
                    break;

                case EventType.ContextClick:
                    OnClickContext.InvokeIfNotNull();
                    break;

                case EventType.MouseDrag:
                    if(IsContextPressed)
                    {
                        DeltaX = Mathf.RoundToInt(Event.current.mousePosition.x - _x);
                        DeltaY = Mathf.RoundToInt(Event.current.mousePosition.y - _y);

                        _x = Event.current.mousePosition.x;
                        _y = Event.current.mousePosition.y;

                        X = Mathf.RoundToInt(Event.current.mousePosition.x);
                        Y = Mathf.RoundToInt(Event.current.mousePosition.y);

                        GlobalX = X + Layout.CanvasX;
                        GlobalY = Y + Layout.CanvasY;

                        OnDragContext.InvokeIfNotNull();
                    }
                    break;
            }
        }
    }
}
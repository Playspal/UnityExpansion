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

        /// <summary>
        /// Mouse X.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Mouse Y.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Is the left mouse button currently pressed.
        /// </summary>
        public bool IsPressed { get; private set; }

        /// <summary>
        /// OnGui event parsing to update mouse state.
        /// </summary>
        public void OnGui()
        {
            switch(Event.current.type)
            {
                case EventType.Repaint:
                    X = (int)Event.current.mousePosition.x;
                    Y = (int)Event.current.mousePosition.y;
                    break;

                case EventType.MouseDown:
                    IsPressed = true;
                    OnPress.InvokeIfNotNull();
                    break;

                case EventType.MouseUp:
                    IsPressed = false;
                    OnRelease.InvokeIfNotNull();
                    OnClick.InvokeIfNotNull();
                    break;

                case EventType.MouseMove:
                    OnMove.InvokeIfNotNull();
                    break;

                case EventType.ContextClick:
                    OnClickContext.InvokeIfNotNull();
                    break;
            }
        }
    }
}
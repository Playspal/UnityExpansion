using System;
using UnityEditor;
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
        public event Action OnMove;

        public event Action OnDragByButtonLeft;
        public event Action OnDragByButtonRight;

        public event Action<UnityEngine.Object[]> OnDragPrefabsStarted;
        public event Action<UnityEngine.Object[]> OnDragPrefabsUpdated;
        public event Action<UnityEngine.Object[]> OnDragPrefabsEnded;
        public event Action OnDragPrefabsCanceled;

        /// <summary>
        /// Parent layout.
        /// </summary>
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
        /// Delta X.
        /// </summary>
        public int DeltaX { get; private set; }

        /// <summary>
        /// Delta Y.
        /// </summary>
        public int DeltaY { get; private set; }

        /// <summary>
        /// X position when left mouse button been pressed last time.
        /// </summary>
        public int PressedX { get; private set; }

        /// <summary>
        /// Y position when left mouse button been pressed last time.
        /// </summary>
        public int PressedY { get; private set; }

        /// <summary>
        /// Is the left mouse button currently pressed.
        /// </summary>
        public bool IsPressedButtonLeft { get; private set; }

        /// <summary>
        /// Is the right mouse button currently pressed.
        /// </summary>
        public bool IsPressedButtonRight { get; private set; }

        /// <summary>
        /// Is the prefabs are currently dragged.
        /// </summary>
        public bool IsPrefabsDragged { get; private set; }

        /// <summary>
        /// Is the mouse currently inside window.
        /// </summary>
        public bool IsInsideLayout { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorLayoutMouse"/> class.
        /// </summary>
        /// <param name="layout">Parent layout.</param>
        public EditorLayoutMouse(EditorLayout layout)
        {
            Layout = layout;
        }

        /// <summary>
        /// OnGui event parsing to update mouse state.
        /// </summary>
        public void OnGui()
        {
            switch (Event.current.type)
            {
                case EventType.Repaint:
                    OnMouseMove();
                    break;

                case EventType.MouseDown:
                    OnMouseDown();
                    break;

                case EventType.MouseUp:
                    OnMouseUp();
                    break;

                case EventType.MouseDrag:
                    OnMouseMove();
                    OnMouseDrag();
                    break;

                case EventType.DragUpdated:
                    OnDragUpdated();
                    Event.current.Use();
                    break;

                case EventType.DragExited:
                    OnDragExit();
                    Event.current.Use();
                    break;
            }
        }

        private void OnMouseMove()
        {
            DeltaX = Mathf.RoundToInt(Event.current.mousePosition.x - X);
            DeltaY = Mathf.RoundToInt(Event.current.mousePosition.y - Y);

            X = Mathf.RoundToInt(Event.current.mousePosition.x);
            Y = Mathf.RoundToInt(Event.current.mousePosition.y);

            IsInsideLayout = X >= 0 && Y >= 0 && X <= Layout.WindowWidth && Y <= Layout.WindowHeight;

            OnMove.InvokeIfNotNull();
        }

        private void OnMouseDown()
        {
            if (Event.current.button == 0)
            {
                PressedX = X;
                PressedY = Y;

                IsPressedButtonLeft = true;
                OnPress.InvokeIfNotNull();
            }

            if (Event.current.button == 1)
            {
                IsPressedButtonRight = true;
            }
        }

        private void OnMouseUp()
        {
            if (Event.current.button == 0)
            {
                IsPressedButtonLeft = false;
                OnRelease.InvokeIfNotNull();

                if (X == PressedX && Y == PressedY)
                {
                    OnClick.InvokeIfNotNull();
                }
            }

            if (Event.current.button == 1)
            {
                IsPressedButtonRight = false;
            }
        }

        private void OnMouseDrag()
        {
            if (IsPressedButtonLeft)
            {
                OnDragByButtonLeft.InvokeIfNotNull();
            }

            if (IsPressedButtonRight)
            {
                OnDragByButtonRight.InvokeIfNotNull();
            }
        }

        private void OnDragUpdated()
        {
            if (DragAndDrop.objectReferences.Length > 0)
            {
                if (!IsPrefabsDragged)
                {
                    IsPrefabsDragged = true;
                    OnDragPrefabsStarted.InvokeIfNotNull(DragAndDrop.objectReferences);
                }

                OnDragPrefabsUpdated.InvokeIfNotNull(DragAndDrop.objectReferences);
            }
        }

        private void OnDragExit()
        {
            if (IsPrefabsDragged)
            {
                IsPrefabsDragged = false;

                if (IsInsideLayout)
                {
                    OnDragPrefabsEnded.InvokeIfNotNull(DragAndDrop.objectReferences);
                }
                else
                {
                    OnDragPrefabsCanceled.InvokeIfNotNull();
                }
            }
        }
    }
}
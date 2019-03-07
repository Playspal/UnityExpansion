using System;
using System.Collections.Generic;

namespace UnityExpansion.Editor
{
    public class EditorLayoutObject
    {
        /// <summary>
        /// Parent layout.
        /// </summary>
        public EditorLayout Layout { get; private set; }

        /// <summary>
        /// Parent object.
        /// </summary>
        public EditorLayoutObject ParentObject { get; private set; }

        /// <summary>
        /// Child objects.
        /// </summary>
        public List<EditorLayoutObject> ChildObjects { get; private set; }

        /// <summary>
        /// Z Index
        /// </summary>
        public float Index { get; set; }

        /// <summary>
        /// Object's local position X.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Object's local position Y.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Object's local position X.
        /// </summary>
        public int GlobalX { get; private set; }

        /// <summary>
        /// Object's local position Y.
        /// </summary>
        public int GlobalY { get; private set; }

        /// <summary>
        /// Object's width.
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Object's height.
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Is object active and visible.
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Is object currently dragged by mouse.
        /// </summary>
        public bool IsDragging { get; private set; }

        /// <summary>
        /// Is object react on mouse.
        /// </summary>
        public bool IsMouseListener { get; private set; }

        /// <summary>
        /// Dispatches on resize
        /// </summary>
        protected event Action OnResize; 

        // Position before drag is started
        private int _dragStartPositionX;
        private int _dragStartPositionY;

        // Drag offset to mouse
        private int _dragOffsetX;
        private int _dragOffsetY;

        public EditorLayoutObject(EditorLayout layout, int width, int height)
        {
            Width = width;
            Height = height;

            SetMouseListener(true);

            Layout = layout;
            Layout.Objects.Add(this);

            ChildObjects = new List<EditorLayoutObject>();

            SetActive(true);
        }

        /// <summary>
        /// Destroys object and all childs.
        /// </summary>
        public virtual void Destroy()
        {
            for (int i = 0; i < ChildObjects.Count; i++)
            {
                ChildObjects[i].Destroy();
            }

            Layout.Objects.Remove(this);
        }


        /// <summary>
        /// Move object to the end of the local objects list.
        /// </summary>
        public void SetAsLastSibling()
        {
            if(ParentObject != null)
            {
                ParentObject.ChildObjects.Remove(this);
                ParentObject.ChildObjects.Add(this);
            }
            else
            {
                Layout.Objects.Remove(this);
                Layout.Objects.Add(this);
            }
        }

        /// <summary>
        /// Move object to the begining of the local objects list.
        /// </summary>
        public void SetAsFirstSibling()
        {
            if (ParentObject != null)
            {
                ParentObject.ChildObjects.Remove(this);
                ParentObject.ChildObjects.Insert(0, this);
            }
        }

        /// <summary>
        /// Allow or disable react on mouse events.
        /// </summary>
        public void SetMouseListener(bool value)
        {
            IsMouseListener = value;
        }

        /// <summary>
        /// Sets the parent object.
        /// </summary>
        public virtual void SetParent(EditorLayoutObject parent)
        {
            if(ParentObject != null)
            {
                ParentObject.ChildObjects.Remove(this);
            }

            ParentObject = parent;

            if (ParentObject != null)
            {
                ParentObject.ChildObjects.Add(this);
            }
        }

        /// <summary>
        /// Sets the object active or not.
        /// </summary>
        public virtual void SetActive(bool value)
        {
            IsActive = value;
        }

        /// <summary>
        /// Resizes object
        /// </summary>
        public virtual void SetSize(int width, int height)
        {
            Width = width;
            Height = height;

            OnResize.InvokeIfNotNull();
        }

        /// <summary>
        /// Starts dragging by Input.mousePosition.
        /// </summary>
        /// <param name="stickToCenter">If true then object's center will match mouse position</param>
        public void DragStart(bool stickToCenter = false)
        {
            if(Layout.ObjectDragged != null)
            {
                return;
            }

            Layout.ObjectDragged = this;

            IsDragging = true;

            _dragStartPositionX = X;
            _dragStartPositionY = Y;

            if (stickToCenter)
            {
                _dragOffsetX = Layout.Mouse.X - GlobalX - Width / 2;
                _dragOffsetY = Layout.Mouse.Y - GlobalY - Height / 2;
            }
            else
            {
                _dragOffsetX = Layout.Mouse.X - GlobalX;
                _dragOffsetY = Layout.Mouse.Y - GlobalY;
            }
        }

        /// <summary>
        /// Stops dragging and return object to original position.
        /// </summary>
        public void DragCancel()
        {
            X = _dragStartPositionX;
            Y = _dragStartPositionY;

            DragStop();
        }

        /// <summary>
        /// Stops dragging.
        /// </summary>
        public void DragStop()
        {
            Layout.ObjectDragged = null;
            IsDragging = false;
        }

        /// <summary>
        /// Is global point inside of the object.
        /// </summary>
        public bool HitTest(int globalX, int globalY)
        {
            return
            (
                globalX >= GlobalX &&
                globalX <= GlobalX + Width &&
                globalY >= GlobalY &&
                globalY <= GlobalY + Height
            );
        }

        /// <summary>
        /// Renders this instance.
        /// Called when object bounds is inside visible window area.
        /// </summary>
        public virtual void Render()
        {
        }

        /// <summary>
        /// Renders this instance.
        /// Called when object bounds is outside visible window area.
        /// </summary>
        public virtual void RenderOntsideOfScreen()
        {
        }

        public virtual void OnMouseOver()
        {
        }

        public virtual void OnMouseOut()
        {
        }

        public virtual void OnMouseMove()
        {
        }

        public virtual void OnMousePress()
        {
        }

        public virtual void OnMouseRelease()
        {
        }

        public virtual void OnMouseReleaseInside()
        {
        }


        public virtual void OnMouseReleaseOutside()
        {
        }

        public virtual void Update()
        {
            if (IsDragging)
            {
                X = Layout.Mouse.X - Layout.CanvasX - _dragOffsetX;
                Y = Layout.Mouse.Y - Layout.CanvasY - _dragOffsetY;
            }

            GlobalX = ParentObject != null ? ParentObject.GlobalX + X : Layout.CanvasX + X;
            GlobalY = ParentObject != null ? ParentObject.GlobalY + Y : Layout.CanvasY + Y;
        }
    }
}
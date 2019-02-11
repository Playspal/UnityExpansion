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
        /// Object's local position X.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Object's local position Y.
        /// </summary>
        public int Y { get; set; }

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
        /// Z Index
        /// </summary>
        public float ZIndex { get; set; }

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
        /// Gets the global position X.
        /// </summary>
        public int GetPositionGlobalX()
        {
            return ParentObject != null ? ParentObject.GetPositionGlobalX() + X : Layout.CanvasX + X;
        }

        /// <summary>
        /// Gets the global position Y.
        /// </summary>
        public int GetPositionGlobalY()
        {
            return ParentObject != null ? ParentObject.GetPositionGlobalY() + Y : Layout.CanvasY + Y;
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
                _dragOffsetX = Layout.Mouse.X - GetPositionGlobalX() - Width / 2;
                _dragOffsetY = Layout.Mouse.Y - GetPositionGlobalY() - Height / 2;
            }
            else
            {
                _dragOffsetX = Layout.Mouse.X - GetPositionGlobalX();
                _dragOffsetY = Layout.Mouse.Y - GetPositionGlobalY();
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
                globalX >= GetPositionGlobalX() &&
                globalX <= GetPositionGlobalX() + Width &&
                globalY >= GetPositionGlobalY() &&
                globalY <= GetPositionGlobalY() + Height
            );
        }

        public virtual void Render()
        {

        }

        public virtual void Update()
        {
            if (IsDragging)
            {
                X = Layout.Mouse.X - Layout.CanvasX - _dragOffsetX;
                Y = Layout.Mouse.Y - Layout.CanvasY - _dragOffsetY;
            }
        }
    }
}
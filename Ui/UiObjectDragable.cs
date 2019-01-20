using UnityEngine;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Provides object dragging functionality.
    /// Uses screen space instead of Canvas and not depends on parents layout and wireframe.
    /// </summary>
    /// <seealso cref="UnityExpansion.UI.UiObject" />
    public class UiObjectDragable : UiObject
    {
        /// <summary>
        /// Is object currently dragged by mouse.
        /// </summary>
        public bool IsDragging { get; private set; }

        /// <summary>
        /// X before drag is started.
        /// </summary>
        public float PositionBeforeDragX { get; private set; }

        /// <summary>
        /// Y before drag is started.
        /// </summary>
        public float PositionBeforeDragY { get; private set; }

        // Offset to mouse
        private float _offsetX;
        private float _offsetY;

        /// <summary>
        /// Starts dragging by Input.mousePosition.
        /// </summary>
        /// <param name="stickToPivot">If true then object's pivot position will match mouse position</param>
        public void DragStart(bool stickToPivot = false)
        {
            IsDragging = true;

            PositionBeforeDragX = X;
            PositionBeforeDragY = Y;

            SetPosition(Input.mousePosition.x, Input.mousePosition.y, true);

            if (!stickToPivot)
            {
                _offsetX = X - PositionBeforeDragX;
                _offsetY = Y - PositionBeforeDragY;
            }
        }

        /// <summary>
        /// Stops dragging and return object to original position.
        /// </summary>
        public void DragCancel()
        {
            X = PositionBeforeDragX;
            Y = PositionBeforeDragY;

            DragStop();
        }

        /// <summary>
        /// Stops dragging.
        /// </summary>
        public void DragStop()
        {
            IsDragging = false;
        }

        /// <summary>
        /// MonoBehavior Update handler.
        /// In inherited classes always use base.Update() when overriding this method.
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if(IsDragging)
            {
                SetPosition(Input.mousePosition.x, Input.mousePosition.y, true);

                X -= _offsetX;
                Y -= _offsetY;
            }
        }
    }
}
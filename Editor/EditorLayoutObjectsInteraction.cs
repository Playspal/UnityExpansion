using System.Collections.Generic;

namespace UnityExpansion.Editor
{
    public class EditorLayoutObjectsInteraction
    {
        private readonly EditorLayoutObjects _objects;
        private readonly EditorLayoutMouse _mouse;

        private List<EditorLayoutObject> _hoveredObjects = new List<EditorLayoutObject>();
        private List<EditorLayoutObject> _pressedObjects = new List<EditorLayoutObject>();

        public EditorLayoutObjectsInteraction(EditorLayoutObjects objects)
        {
            _objects = objects;

            _mouse = _objects.Layout.Mouse;
            _mouse.OnMove += OnMouseMove;
            _mouse.OnPress += OnMousePress;
            _mouse.OnRelease += OnMouseRelease;
        }

        private void OnMouseMove()
        {
            List<EditorLayoutObject> hitTested = _objects.HitTestAll(_mouse.X, _mouse.Y);

            for (int i = 0; i < _hoveredObjects.Count; i++)
            {
                if (!hitTested.Contains(_hoveredObjects[i]))
                {
                    _hoveredObjects[i].OnMouseOut();
                    _hoveredObjects[i] = null;
                }
            }

            for (int i = 0; i < hitTested.Count; i++)
            {
                if (!_hoveredObjects.Contains(hitTested[i]))
                {
                    hitTested[i].OnMouseOver();
                    _hoveredObjects.Add(hitTested[i]);
                }
                else
                {
                    hitTested[i].OnMouseMove();
                }
            }

            _hoveredObjects.RemoveAll(x => x == null);
        }

        private void OnMousePress()
        {
            List<EditorLayoutObject> hitTested = _objects.HitTestAll(_mouse.X, _mouse.Y);

            for(int i = 0; i < hitTested.Count; i++)
            {
                hitTested[i].OnMousePress();
                _pressedObjects.Add(hitTested[i]);
            }
        }

        private void OnMouseRelease()
        {
            List<EditorLayoutObject> hitTested = _objects.HitTestAll(_mouse.X, _mouse.Y);

            for(int i = 0; i < _pressedObjects.Count; i++)
            {
                _pressedObjects[i].OnMouseRelease();

                if (hitTested.Contains(_pressedObjects[i]))
                {
                    _pressedObjects[i].OnMouseReleaseInside();
                }
                else
                {
                    _pressedObjects[i].OnMouseReleaseOutside();
                }
            }

            _pressedObjects.Clear();
        }
    }
}
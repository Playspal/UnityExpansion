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
                    _hoveredObjects[i].ProcessEvent(EditorLayoutObjectEvent.MouseOut);
                    _hoveredObjects[i] = null;
                }
            }

            for (int i = 0; i < hitTested.Count; i++)
            {
                if (!_hoveredObjects.Contains(hitTested[i]))
                {
                    hitTested[i].ProcessEvent(EditorLayoutObjectEvent.MouseOver);
                    _hoveredObjects.Add(hitTested[i]);
                }
                else
                {
                    hitTested[i].ProcessEvent(EditorLayoutObjectEvent.MouseMove);
                }
            }

            _hoveredObjects.RemoveAll(x => x == null);
        }

        private void OnMousePress()
        {
            List<EditorLayoutObject> hitTested = _objects.HitTestAll(_mouse.X, _mouse.Y);

            for(int i = 0; i < hitTested.Count; i++)
            {
                hitTested[i].ProcessEvent(EditorLayoutObjectEvent.MousePress);
                _pressedObjects.Add(hitTested[i]);
            }
        }

        private void OnMouseRelease()
        {
            List<EditorLayoutObject> hitTested = _objects.HitTestAll(_mouse.X, _mouse.Y);

            for(int i = 0; i < _pressedObjects.Count; i++)
            {
                _pressedObjects[i].ProcessEvent(EditorLayoutObjectEvent.MouseRelease);
                _pressedObjects[i].ProcessEvent
                (
                    hitTested.Contains(_pressedObjects[i]) ?
                    EditorLayoutObjectEvent.MouseReleaseInside :
                    EditorLayoutObjectEvent.MouseReleaseOutside
                );
            }

            _pressedObjects.Clear();
        }
    }
}
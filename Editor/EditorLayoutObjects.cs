using System.Collections.Generic;

namespace UnityExpansion.Editor
{
    /// <summary>
    /// This class required to controll and render LayoutObjects.
    /// Normally instance created and used by EditorLayout.
    /// </summary>
    public class EditorLayoutObjects
    {
        /// <summary>
        /// Parent EditorLayout object.
        /// </summary>
        public EditorLayout Layout { get; private set; }

        private List<EditorLayoutObject> _objects = new List<EditorLayoutObject>();
        private List<EditorLayoutObject> _objectsToAdd = new List<EditorLayoutObject>();
        private List<EditorLayoutObject> _objectsToRemove = new List<EditorLayoutObject>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorLayoutObjects"/> class.
        /// </summary>
        public EditorLayoutObjects(EditorLayout layout)
        {
            Layout = layout;
        }

        /// <summary>
        /// Destroys all objects.
        /// </summary>
        public void DestroyAllObjects()
        {
            for(int i = 0; i < _objects.Count; i++)
            {
                _objects[i].Destroy();
            }
        }

        /// <summary>
        /// Adds layout object.
        /// </summary>
        public void Add(EditorLayoutObject layoutObject)
        {
            _objectsToAdd.Add(layoutObject);
        }

        /// <summary>
        /// Removes layout object.
        /// </summary>
        public void Remove(EditorLayoutObject layoutObject)
        {
            _objectsToRemove.Add(layoutObject);
        }

        /// <summary>
        /// Finds all objects with specified type.
        /// </summary>
        public List<T> FindAllObjects<T>() where T : EditorLayoutObject
        {
            List<T> output = new List<T>();

            for(int i = 0; i < _objects.Count; i++)
            {
                if(_objects[i] is T)
                {
                    output.Add(_objects[i] as T);
                }
            }

            return output;
        }

        /// <summary>
        /// Renders all LayoutObjects according to hierarchy.
        /// </summary>
        public void Render()
        {
            for (int i = 0; i < _objectsToRemove.Count; i++)
            {
                _objects.Remove(_objectsToRemove[i]);
            }

            for (int i = 0; i < _objectsToAdd.Count; i++)
            {
                _objects.Add(_objectsToAdd[i]);
            }

            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i].ParentObject == null)
                {
                    RenderRecursively(_objects[i]);
                }
            }

            _objectsToRemove.Clear();
            _objectsToAdd.Clear();
        }

        public void Update()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i].ParentObject == null)
                {
                    UpdateRecursively(_objects[i]);
                }
            }
        }

        private void RenderRecursively(EditorLayoutObject layoutObject)
        {
            if (layoutObject.IsActive)
            {
                if
                (
                    layoutObject.GlobalX < Layout.WindowWidth &&
                    layoutObject.GlobalX + layoutObject.Width > 0 &&
                    layoutObject.GlobalY < Layout.WindowHeight &&
                    layoutObject.GlobalY + layoutObject.Height > 0
                )
                {
                    layoutObject.Render();
                }
                else
                {
                    layoutObject.RenderOntsideOfScreen();
                }

                for (int i = 0; i < layoutObject.ChildObjects.Count; i++)
                {
                    RenderRecursively(layoutObject.ChildObjects[i]);
                }
            }
        }

        private void UpdateRecursively(EditorLayoutObject layoutObject)
        {
            if (layoutObject.IsActive)
            {
                layoutObject.Update();

                for (int i = 0; i < layoutObject.ChildObjects.Count; i++)
                {
                    UpdateRecursively(layoutObject.ChildObjects[i]);
                }
            }
        }
    }
}
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

        private EditorLayoutObjectsInteraction _interaction;

        private List<EditorLayoutObject> _objects = new List<EditorLayoutObject>();
 
        private int _nextZindex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorLayoutObjects"/> class.
        /// </summary>
        public EditorLayoutObjects(EditorLayout layout)
        {
            Layout = layout;

            _interaction = new EditorLayoutObjectsInteraction(this);
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
            _objects.Add(layoutObject);
        }

        /// <summary>
        /// Removes layout object.
        /// </summary>
        public void Remove(EditorLayoutObject layoutObject)
        {
            _objects.Remove(layoutObject);
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
        /// Get list of objects that collide with provided global position.
        /// </summary>
        public List<EditorLayoutObject> HitTestAll(int x, int y)
        {
            List<EditorLayoutObject> output = new List<EditorLayoutObject>();
            EditorLayoutObject layoutObject = HitTest(Layout.Mouse.X, Layout.Mouse.Y);

            while (layoutObject != null)
            {
                output.Add(layoutObject);
                layoutObject = layoutObject.ParentObject;
            }

            return output;
        }

        /// <summary>
        /// Get object that collide with provided global position.
        /// </summary>
        public EditorLayoutObject HitTest(int x, int y)
        {
            EditorLayoutObject output = null;

            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i].IsMouseListener && _objects[i].HitTest(x, y))
                {
                    if (output == null || _objects[i].Index > output.Index)
                    {
                        output = _objects[i];
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Renders all LayoutObjects according to hierarchy.
        /// </summary>
        public void Render()
        {
            ZIndexReset();

            EditorLayoutObject[] objects = _objects.ToArray();

            for (int i = 0; i < objects.Length; i++)
            {
                if (_objects[i].ParentObject == null)
                {
                    RenderRecursively(_objects[i]);
                }
            }
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
                ZIndexAssign(layoutObject);

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

        private void ZIndexReset()
        {
            _nextZindex = 0;
        }

        private void ZIndexAssign(EditorLayoutObject layoutObject)
        {
            layoutObject.Index = _nextZindex;
            _nextZindex++;
        }
    }
}
using UnityEditor;

namespace UnityExpansion.Editor
{
    public class EditorLayout : EditorWindow
    {
        public int WindowWidth { get { return (int)(position.width); } }
        public int WindowHeight { get { return (int)(position.height); } }

        public EditorLayoutMouse Mouse { get; private set; }
        public EditorLayoutObjects Objects { get; private set; }

        public virtual void Initialization()
        {
            Mouse = new EditorLayoutMouse();
            Objects = new EditorLayoutObjects(this);
        }

        protected virtual void OnGUI()
        {
            Mouse.OnGui();
            Objects.Render();
        }

        private void Update()
        {
            Repaint();
        }
    }
}
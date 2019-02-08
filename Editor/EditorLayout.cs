using UnityEditor;

namespace UnityExpansion.Editor
{
    public class EditorLayout : EditorWindow
    {
        public int WindowWidth { get { return (int)(position.width); } }
        public int WindowHeight { get { return (int)(position.height); } }

        public int CanvasX { get; set; }
        public int CanvasY { get; set; }

        public EditorLayoutMouse Mouse { get; private set; }
        public EditorLayoutObjects Objects { get; private set; }

        public EditorLayoutObject ObjectDragged { get; set; }

        public virtual void Initialization()
        {
            Mouse = new EditorLayoutMouse(this);
            Objects = new EditorLayoutObjects(this);
        }

        protected virtual void OnGUI()
        {
            Mouse.OnGui();
            Objects.Render();
            Objects.Update();
        }

        private void Update()
        {
            
            Repaint();
        }
    }
}
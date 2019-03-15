using UnityEditor;
using UnityEngine;

namespace UnityExpansion.Editor
{
    public class EditorLayoutObjectReference : EditorLayoutObject
    {
        // TODO: finish implementation
        public EditorLayoutObjectReference(EditorLayout window, int width, int height) : base(window, width, height)
        {
        }

        public override void Render()
        {
            base.Render();
            EditorGUI.ObjectField(new Rect(GlobalX, GlobalY, Width, Height), "", null, typeof(GameObject), true);
        }
    }
}

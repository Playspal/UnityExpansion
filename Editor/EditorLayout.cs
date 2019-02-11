﻿using System;

using UnityEditor;

namespace UnityExpansion.Editor
{
    public class EditorLayout : EditorWindow
    {
        public event Action OnWindowResized;
        public event Action<UnityEngine.Object> OnObjectDraggedIn;

        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }
        public int CanvasX { get; set; }
        public int CanvasY { get; set; }

        public EditorLayoutMouse Mouse { get; private set; }
        public EditorLayoutObjects Objects { get; private set; }
        public EditorLayoutObject ObjectDragged { get; set; }

        public virtual void Initialization()
        {
            UpdateSize();

            Mouse = new EditorLayoutMouse(this);
            Objects = new EditorLayoutObjects(this);
        }

        protected virtual void OnGUI()
        {
            UpdateSize();

            Mouse.OnGui();

            Objects.Render();
            Objects.Update();
        }

        protected virtual void Update()
        {
            Repaint();
        }

        private void UpdateSize()
        {
            int width = (int)(position.width);
            int height = (int)(position.height);

            if(WindowWidth != width || WindowHeight != height)
            {
                WindowWidth = width;
                WindowHeight = height;

                OnWindowResized.InvokeIfNotNull();
            }
        }
    }
}
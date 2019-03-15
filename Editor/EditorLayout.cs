using System;

using UnityEditor;
using UnityEngine;

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
        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }
        public float CanvasScale { get; private set; }

        public EditorLayoutMouse Mouse { get; private set; }
        public EditorLayoutObjects Objects { get; private set; }
        public EditorLayoutObject ObjectDragged { get; set; }
        
        public virtual void Initialization()
        {
            UpdateSize();
            SetScale(1, false);

            Mouse = new EditorLayoutMouse(this);
            Objects = new EditorLayoutObjects(this);
        }

        public void EnableZoomByMouseWheel(float min, float max, float step)
        {
            Mouse.OnScroll += (float delta) =>
            {
                float scale = CanvasScale + (delta > 0 ? -step : step);
                SetScale(Mathf.Clamp(scale, min, max));
            };
        }

        public void SetScale(float value, bool followMouse = true)
        {
            int canvasWidthBefore = CanvasWidth != 0 ? CanvasWidth : WindowWidth;
            int canvasHeightBefore = CanvasHeight != 0 ? CanvasHeight : WindowHeight;

            CanvasScale = value;

            float scale = (float)WindowWidth / ((float)WindowWidth * CanvasScale);

            CanvasWidth = Mathf.RoundToInt(WindowWidth * scale);
            CanvasHeight = Mathf.RoundToInt(WindowHeight * scale);

            int deltaX = CanvasWidth - canvasWidthBefore;
            int deltaY = CanvasHeight - canvasHeightBefore;

            if (followMouse)
            {
                float ratioX = !followMouse ? 0 : (float)Mouse.X / (float)canvasWidthBefore;
                float ratioY = !followMouse ? 0 : (float)Mouse.Y / (float)canvasHeightBefore;

                CanvasX += Mathf.RoundToInt(deltaX * ratioX);
                CanvasY += Mathf.RoundToInt(deltaY * ratioY);
            }
            else
            {
                CanvasX += deltaX / 2;
                CanvasY += deltaY / 2;
            }
        }

        protected virtual void OnDestroy()
        {
            Objects.DestroyAllObjects();

            EditorLayoutObjectTextureCachable.ClearCache();
            Resources.UnloadUnusedAssets();
        }

        protected virtual void OnGUI()
        {
            UpdateSize();

            float scale = (float)WindowWidth / ((float)WindowWidth * CanvasScale);

            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0, 23, Mathf.RoundToInt((float)WindowWidth * scale), Mathf.RoundToInt((float)WindowHeight * scale)));

            Matrix4x4 oldMatrix = GUI.matrix;
            Vector2 vanishingPoint = new Vector2(0, 21);

            Matrix4x4 Translation = Matrix4x4.TRS(vanishingPoint, Quaternion.identity, Vector3.one);
            Matrix4x4 Scale = Matrix4x4.Scale(new Vector3(CanvasScale, CanvasScale, 1.0f));
            GUI.matrix = Translation * Scale * Translation.inverse;

            Render();

            Mouse.OnGui();

            GUI.matrix = oldMatrix;
        }

        protected virtual void Render()
        {
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
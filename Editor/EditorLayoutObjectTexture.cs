using UnityEngine;

namespace UnityExpansion.Editor
{
    public class EditorLayoutObjectTexture : EditorLayoutObject
    {
        public Texture2D Texture { get; protected set; }
        public int Scale { get; protected set; }
        public EditorLayoutObjectTexture(EditorLayout layout, int width, int height) : base(layout, width, height)
        {
            Scale = 1;
            Clear();
        }

        public override void Destroy()
        {
            base.Destroy();

            Texture2D.DestroyImmediate(Texture, true);
        }

        public void Clear()
        {
            Texture = new Texture2D(Width * Scale, Height * Scale);
        }

        public void SetScale(int value)
        {
            Scale = value;
            Clear();
        }

        public override void SetSize(int width, int height)
        {
            base.SetSize(width, height);
            Clear();
        }

        public override void Render()
        {
            base.Render();

            Rect bounds = new Rect(GlobalX, GlobalY, Width, Height);
            GUI.DrawTexture(bounds, Texture, ScaleMode.StretchToFill, true, 1f);
        }

        /// <summary>
        /// Fills whole texture.
        /// </summary>
        public void Fill(Color color)
        {
            Texture.DrawRect(0, 0, Texture.width, Texture.height, color);
        }

        /// <summary>
        /// Draws the border.
        /// </summary>
        public void DrawBorder(int thickness, Color color)
        {
            Texture.DrawBorderTop(thickness, color);
            Texture.DrawBorderBottom(thickness, color);
            Texture.DrawBorderLeft(thickness, color);
            Texture.DrawBorderRight(thickness, color);
        }

        /// <summary>
        /// Draws the border top.
        /// </summary>
        public void DrawBorderTop(int thickness, Color color)
        {
            Texture.DrawRect(0, Texture.height - thickness, Texture.width, thickness, color);
        }

        /// <summary>
        /// Draws the border bottom.
        /// </summary>
        public void DrawBorderBottom(int thickness, Color color)
        {
            Texture.DrawRect(0, 0, Texture.width, thickness, color);
        }

        /// <summary>
        /// Draws the border left.
        /// </summary>
        public void DrawBorderLeft(int thickness, Color color)
        {
            Texture.DrawRect(0, 0, thickness, Texture.height, color);
        }

        /// <summary>
        /// Draws the border right.
        /// </summary>
        public void DrawBorderRight(int thickness, Color color)
        {
            Texture.DrawRect(Texture.width - thickness, 0, thickness, Texture.height, color);
        }

        /// <summary>
        /// Draws rectangle in texture.
        /// </summary>
        public void DrawRect(int x, int y, int width, int height, Color color)
        {
            Texture.DrawRect(x, y, width, height, color);
        }

        /// <summary>
        /// Draws rhombus in texture.
        /// </summary>
        public void DrawRhombus(int x, int y, int radius, Color color)
        {
            Texture.DrawRhombus(x * Scale, y * Scale, radius * Scale, color);
        }

        /// <summary>
        /// Draws rhombus in texture.
        /// </summary>
        public void DrawTriangleUp(int x, int y, int radius, Color color)
        {
            Texture.DrawTriangleUp(x * Scale, y * Scale, radius * Scale, color);
        }

        /// <summary>
        /// Draws rhombus in texture.
        /// </summary>
        public void DrawTriangleDown(int x, int y, int radius, Color color)
        {
            Texture.DrawTriangleDown(x * Scale, y * Scale, radius * Scale, color);
        }

        /// <summary>
        /// Draws circle in texture.
        /// </summary>
        public void DrawCircle(int x, int y, int radius, Color color)
        {
            Texture.DrawCircle(x * Scale, y * Scale, radius * Scale, color);
        }
    }
}
using UnityEngine;

namespace UnityExpansion.Editor
{
    public class EditorLayoutObjectTexture : EditorLayoutObject
    {
        public Texture2D Texture { get; private set; }

        public EditorLayoutObjectTexture(EditorLayout layout, int width, int height) : base(layout, width, height)
        {
            Texture = new Texture2D(Width, Height);
        }

        public override void Render()
        {
            base.Render();

            Rect bounds = new Rect(GetPositionGlobalX(), GetPositionGlobalY(), Width, Height);
            GUI.DrawTexture(bounds, Texture, ScaleMode.StretchToFill, true, 1f);
        }

        /// <summary>
        /// Fills whole texture.
        /// </summary>
        public void Fill(Color color)
        {
            Texture.DrawRect(0, 0, Width, Height, color);
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
            Texture.DrawRect(0, Height - thickness, Width, thickness, color);
        }

        /// <summary>
        /// Draws the border bottom.
        /// </summary>
        public void DrawBorderBottom(int thickness, Color color)
        {
            Texture.DrawRect(0, 0, Width, thickness, color);
        }

        /// <summary>
        /// Draws the border left.
        /// </summary>
        public void DrawBorderLeft(int thickness, Color color)
        {
            Texture.DrawRect(0, 0, thickness, Height, color);
        }

        /// <summary>
        /// Draws the border right.
        /// </summary>
        public void DrawBorderRight(int thickness, Color color)
        {
            Texture.DrawRect(Width - thickness, 0, thickness, Height, color);
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
            Texture.DrawRhombus(x, y, radius, color);
        }

        /// <summary>
        /// Draws circle in texture.
        /// </summary>
        public void DrawCircle(int x, int y, int radius, Color color)
        {
            Texture.DrawCircle(x, y, radius, color);
        }
    }
}
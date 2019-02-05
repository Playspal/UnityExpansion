using UnityEngine;

namespace UnityExpansion.Editor
{
    public class EditorLayoutTexture2D : EditorLayoutObject
    {
        public Texture2D Texture { get; private set; }

        public EditorLayoutTexture2D(EditorLayout layout, int width, int height) : base(layout, width, height)
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
        public void Fill(string color)
        {
            DrawRect(0, 0, Width, Height, color);
        }

        /// <summary>
        /// Fills whole texture.
        /// </summary>
        public void Fill(Color color)
        {
            DrawRect(0, 0, Width, Height, color);
        }

        /// <summary>
        /// Draws the border.
        /// </summary>
        public void DrawBorder(int thickness, string color) { DrawBorder(thickness, ParseColor(color)); }

        /// <summary>
        /// Draws the border.
        /// </summary>
        public void DrawBorder(int thickness, Color color)
        {
            DrawBorderTop(thickness, color);
            DrawBorderBottom(thickness, color);
            DrawBorderLeft(thickness, color);
            DrawBorderRight(thickness, color);
        }

        /// <summary>
        /// Draws the border top.
        /// </summary>
        public void DrawBorderTop(int thickness, string color) { DrawBorderTop(thickness, ParseColor(color)); }

        /// <summary>
        /// Draws the border top.
        /// </summary>
        public void DrawBorderTop(int thickness, Color color)
        {
            DrawRect(0, Height - thickness, Width, thickness, color);
        }

        /// <summary>
        /// Draws the border bottom.
        /// </summary>
        public void DrawBorderBottom(int thickness, string color) { DrawBorderBottom(thickness, ParseColor(color)); }

        /// <summary>
        /// Draws the border bottom.
        /// </summary>
        public void DrawBorderBottom(int thickness, Color color)
        {
            DrawRect(0, 0, Width, thickness, color);
        }

        /// <summary>
        /// Draws the border left.
        /// </summary>
        public void DrawBorderLeft(int thickness, string color) { DrawBorderLeft(thickness, ParseColor(color)); }

        /// <summary>
        /// Draws the border left.
        /// </summary>
        public void DrawBorderLeft(int thickness, Color color)
        {
            DrawRect(0, 0, thickness, Height, color);
        }

        /// <summary>
        /// Draws the border right.
        /// </summary>
        public void DrawBorderRight(int thickness, string color) { DrawBorderRight(thickness, ParseColor(color)); }

        /// <summary>
        /// Draws the border right.
        /// </summary>
        public void DrawBorderRight(int thickness, Color color)
        {
            DrawRect(Width - thickness, 0, thickness, Height, color);
        }

        /// <summary>
        /// Draws rhombus in texture.
        /// </summary>
        public void DrawRhombus(int x, int y, int size, string color)
        {
            DrawRhombus(x, y, size, ParseColor(color));
        }

        /// <summary>
        /// Draws rhombus in texture.
        /// </summary>
        public void DrawRhombus(int x, int y, int size, Color color)
        {
            Color transparent = new Color(0, 0, 0, 0);

            int sizeHalf = Mathf.CeilToInt((float)size / 2f);

            for (int i = 0; i < sizeHalf; i++)
            {
                int xFrom = x - i;
                int xTo = x + i;
                int y1 = y + (sizeHalf - i) - 1;
                int y2 = y - (sizeHalf - i) + 1;

                DrawRect(xFrom, y1, i * 2 + 1, 1, color);
                DrawRect(xFrom, y2, i * 2 + 1, 1, color);
            }

            Texture.Apply();
        }

        /// <summary>
        /// Draws rectangle in texture.
        /// </summary>
        public void DrawRect(int x, int y, int width, int height, string color)
        {
            DrawRect(x, y, width, height, ParseColor(color));
        }

        /// <summary>
        /// Draws rectangle in texture.
        /// </summary>
        public void DrawRect(int x, int y, int width, int height, Color color)
        {
            int fromX = ClampX(x);
            int fromY = ClampY(y);
            int toX = ClampX(x + Mathf.Abs(width));
            int toY = ClampY(y + Mathf.Abs(height));

            for (int pixelX = fromX; pixelX < toX; pixelX++)
            {
                for (int pixelY = fromY; pixelY < toY; pixelY++)
                {
                    Texture.SetPixel(pixelX, pixelY, color);
                }
            }

            Texture.Apply();
        }

        /// <summary>
        /// Draws the line.
        /// </summary>
        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            x1 = ClampX(x1);
            y1 = ClampY(y1);
            x2 = ClampX(x2);
            y2 = ClampY(y2);
        }

        private int ClampX(int value)
        {
            return Mathf.Clamp(value, 0, Width);
        }

        private int ClampY(int value)
        {
            return Mathf.Clamp(value, 0, Height);
        }

        private Color ParseColor(string value)
        {
            Color color = Color.red.Parse(value);

            //ColorUtility.TryParseHtmlString(value, out color);

            return color;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsTexture2D
{
    /// <summary>
    /// Fills whole texture.
    /// </summary>
    public static void Fill(this Texture2D texture, Color color)
    {
        texture.DrawRect(0, 0, texture.width, texture.height, color);
    }

    /// <summary>
    /// Draws the border.
    /// </summary>
    public static void DrawBorder(this Texture2D texture, int thickness, Color color)
    {
        texture.DrawBorderTop(thickness, color);
        texture.DrawBorderBottom(thickness, color);
        texture.DrawBorderLeft(thickness, color);
        texture.DrawBorderRight(thickness, color);
    }

    /// <summary>
    /// Draws the border top.
    /// </summary>
    public static void DrawBorderTop(this Texture2D texture, int thickness, Color color)
    {
        texture.DrawRect(0, texture.height - thickness, texture.width, thickness, color);
    }

    /// <summary>
    /// Draws the border bottom.
    /// </summary>
    public static void DrawBorderBottom(this Texture2D texture, int thickness, Color color)
    {
        texture.DrawRect(0, 0, texture.width, thickness, color);
    }

    /// <summary>
    /// Draws the border left.
    /// </summary>
    public static void DrawBorderLeft(this Texture2D texture, int thickness, Color color)
    {
        texture.DrawRect(0, 0, thickness, texture.height, color);
    }

    /// <summary>
    /// Draws the border right.
    /// </summary>
    public static void DrawBorderRight(this Texture2D texture, int thickness, Color color)
    {
        texture.DrawRect(texture.width - thickness, 0, thickness, texture.height, color);
    }

    /// <summary>
    /// Draws rectangle in texture.
    /// </summary>
    public static void DrawRect(this Texture2D texture, int x, int y, int width, int height, Color color, bool apply = true)
    {
        int fromX = x;
        int fromY = y;
        int toX = x + width;
        int toY = y + height;

        for (int pixelX = fromX; pixelX < toX; pixelX++)
        {
            for (int pixelY = fromY; pixelY < toY; pixelY++)
            {
                texture.SetPixel(pixelX, pixelY, color);
            }
        }

        if (apply)
        {
            texture.Apply();
        }
    }

    /// <summary>
    /// Draws rhombus in texture.
    /// </summary>
    public static void DrawRhombus(this Texture2D texture, int x, int y, int radius, Color color)
    {
        Color transparent = new Color(0, 0, 0, 0);

        for (int i = 0; i < radius; i++)
        {
            int xFrom = x - i;
            int xTo = x + i;
            int y1 = y + (radius - i) - 1;
            int y2 = y - (radius - i) + 1;

            texture.DrawRect(xFrom, y1, i * 2 + 1, 1, color, false);
            texture.DrawRect(xFrom, y2, i * 2 + 1, 1, color, false);
        }

        texture.Apply();
    }

    /// <summary>
    /// Draws triangle in texture.
    /// </summary>
    public static void DrawTriangleUp(this Texture2D texture, int x, int y, int radius, Color color)
    {
        Color transparent = new Color(0, 0, 0, 0);

        for (int i = 0; i < radius; i++)
        {
            int xFrom = x - i;
            int xTo = x + i;
            int y1 = y + (radius - i);

            texture.DrawRect(xFrom, y1, i * 2 + 1, 1, color, false);
        }

        texture.Apply();
    }

    /// <summary>
    /// Draws triangle in texture.
    /// </summary>
    public static void DrawTriangleDown(this Texture2D texture, int x, int y, int radius, Color color)
    {
        Color transparent = new Color(0, 0, 0, 0);

        for (int i = 0; i < radius; i++)
        {
            int xFrom = x - i;
            int xTo = x + i;
            int y2 = y - (radius - i);

            texture.DrawRect(xFrom, y2, i * 2 + 1, 1, color, false);
        }

        texture.Apply();
    }

    /// <summary>
    /// Draws circle.
    /// </summary>
    public static void DrawCircle(this Texture2D texture, int x, int y, int radius, Color color)
    {
        for(float a = 0; a < 180; a++)
        {
            float angle = a / 180f * Mathf.PI;

            int xTemp = Mathf.RoundToInt(Mathf.Cos(angle) * radius);
            int yTemp = Mathf.RoundToInt(Mathf.Sin(angle) * radius);

            int xFrom = x + xTemp;
            int yFrom = y - yTemp;

            int xTo = x + xTemp;
            int yTo = y - yTemp;

            texture.DrawRect(xFrom, yFrom, 1, yTemp * 2 + 1, color, false);
        }

        texture.Apply();
    }
}

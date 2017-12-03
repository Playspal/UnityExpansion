#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityExpansion.Tweens;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal
{
    public static class InternalUiAnimationEditorTextures
    {
        public static Texture2D ColorInvisible;

        public static Texture2D ColorBlack100;
        public static Texture2D ColorBlack20;
        public static Texture2D ColorBlack10;

        public static Texture2D ColorWhite100;
        public static Texture2D ColorWhite20;

        public static Texture2D ColorRed;

        public static void Setup()
        {
            ColorInvisible = CreateIfNull(ColorInvisible, 0, 0, 0, 0);

            ColorBlack100 = CreateIfNull(ColorBlack100, 0, 0, 0, 1);
            ColorBlack20 = CreateIfNull(ColorBlack20, 0, 0, 0, 0.2f);
            ColorBlack10 = CreateIfNull(ColorBlack10, 0, 0, 0, 0.1f);

            ColorWhite100 = CreateIfNull(ColorWhite100, 255, 255, 255, 1f);
            ColorWhite20 = CreateIfNull(ColorWhite20, 255, 255, 255, 0.2f);
        }


        private static Texture2D CreateIfNull(Texture2D original, float r, float g, float b, float a)
        {
            if (original != null)
            {
                return original;
            }

            return Create(r, g, b, a);
        }

        private static Texture2D Create(float r, float g, float b, float a)
        {
            Color color = new Color(r / 255, g / 255, b / 255, a);

            int width = 128;
            int height = 128;

            Texture2D output = new Texture2D(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    output.SetPixel(x, y, color);
                }
            }

            output.Apply();

            return output;
        }

        private static string GetColorId(Color color)
        {
            return GetColorId(color.r, color.g, color.b, color.a);
        }

        private static string GetColorId(float r, float g, float b, float a)
        {
            return r + ";" + g + ";" + b + ";" + a;
        }

        public static Dictionary<string, Texture2D> ItemsBackgroundsCache = new Dictionary<string, Texture2D>();

        public static Texture2D GenerateCanvasItemBackground(int width, int height, UiAnimationClipSegmentType type, EasingType processorType, bool transparent = false)
        {
            string id = width + ":" + height + ":" + type.ToString() + ":" + processorType.ToString() + ":" + (transparent ? "transparent" : "normal");

            if (ItemsBackgroundsCache.ContainsKey(id))
            {
                return ItemsBackgroundsCache[id];
            }

            Color colorA = Color.black;
            Color colorB = Color.black;
            Color colorC = Color.black;

            switch (type)
            {
                case UiAnimationClipSegmentType.Alpha:
                    colorA = new Color(84f / 255f, 152f / 255f, 199f / 255f, 1f);
                    colorB = new Color(118f / 255f, 174f / 255f, 211f / 255f, 1f);
                    colorC = new Color(67f / 255f, 123f / 255f, 160f / 255f, 1f);
                    break;

                case UiAnimationClipSegmentType.Color:
                    colorA = new Color(36f / 255f, 174f / 255f, 96f / 255f, 1f);
                    colorB = new Color(80f / 255f, 190f / 255f, 129f / 255f, 1f);
                    colorC = new Color(29f / 255f, 139f / 255f, 78f / 255f, 1f);
                    break;

                case UiAnimationClipSegmentType.Position:
                    colorA = new Color(192f / 255f, 56f / 255f, 44f / 255f, 1f);
                    colorB = new Color(207f / 255f, 96f / 255f, 77f / 255f, 1f);
                    colorC = new Color(153f / 255f, 45f / 255f, 35f / 255f, 1f);
                    break;

                case UiAnimationClipSegmentType.Rotation:
                    colorA = new Color(210f / 255f, 84f / 255f, 0f / 255f, 1f);
                    colorB = new Color(210f / 255f, 119f / 255f, 51f / 255f, 1f);
                    colorC = new Color(169f / 255f, 67f / 255f, 1f / 255f, 1f);
                    break;

                case UiAnimationClipSegmentType.Scale:
                    colorA = new Color(243f / 255f, 156f / 255f, 14f / 255f, 1f);
                    colorB = new Color(247f / 255f, 192f / 255f, 101f / 255f, 1f);
                    colorC = new Color(196f / 255f, 141f / 255f, 50f / 255f, 1f);
                    break;
            }

            Texture2D texture = new Texture2D(width, height);

            if (transparent)
            {
                colorA.a = 0.4f;
                colorB.a = 0.4f;
                colorC.a = 0.4f;
            }

            // Background
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    //colorA.a = isLine ? 0.2f : 1;

                    texture.SetPixel(x, y, colorA);
                }
            }

            // Border top
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, height - 1, colorB);
            }


            // Border left
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(0, y, colorB);
            }

            // Border bottom
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, 0, colorC);
            }

            // Border right
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(width - 1, y, colorC);
            }

            // Curve
            for (int i = 0; i < width; i++)
            {
                float value = Easing.Interpolate((float)i / (float)width, processorType) * (float)(height - 2) / 2;

                if (transparent)
                {
                    value = 0;
                }

                int x = i;
                int y = (int)(height / 4 + value);

                texture.SetPixel(i, y + 1, colorB);
                texture.SetPixel(i, y, colorC);
            }

            texture.Apply();

            ItemsBackgroundsCache.Add(id, texture);

            return texture;
        }
    }
}
#endif
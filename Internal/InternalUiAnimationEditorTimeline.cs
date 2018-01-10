#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal
{
    public class InternalUiAnimationEditorTimeline
    {
        public static float Scale = 0.5f;

        public static int Height = 21;

        /// <summary>
        /// Actual width of timeline.
        /// </summary>
        public static int Width
        {
            get
            {
                return (int)(InternalUiAnimationEditor.WindowWidth - InternalUiAnimationEditorInspector.Width);
            }
        }

        /// <summary>
        /// Amount of visible segments.
        /// </summary>
        public static int SegmentsCount
        {
            get
            {
                return Mathf.RoundToInt(Scale * 10 * 10);
            }
        }

        /// <summary>
        /// Width of single segment.
        /// </summary>
        public static float SegmentWidth
        {
            get
            {
                return (float)Width / (float)SegmentsCount;
            }
        }

        public static void Update()
        {
        }

        public static void OnEvent()
        {
            if (Event.current.type == EventType.ScrollWheel)
            {
                if (Event.current.delta.y > 0)
                {
                    if (Scale < 1)
                    {
                        Scale += 0.1f;
                    }
                }

                if (Event.current.delta.y < 0)
                {
                    if (Scale >= 0.2f)
                    {
                        Scale -= 0.1f;
                    }
                }
            }
        }

        public static void Render(float positionX, float positionY, float width, float height)
        {
            InternalUiAnimationEditorGUI.DrawTexture(0, positionY, InternalUiAnimationEditor.WindowWidth, height, InternalUiAnimationEditorTextures.ColorWhite20);
            InternalUiAnimationEditorGUI.DrawTexture(0, positionY + height - 2, InternalUiAnimationEditor.WindowWidth, 1, InternalUiAnimationEditorTextures.ColorWhite20);
            InternalUiAnimationEditorGUI.DrawTexture(0, positionY + height - 1, InternalUiAnimationEditor.WindowWidth, 1, InternalUiAnimationEditorTextures.ColorBlack20);

            RenderMenu(0, 0, InternalUiAnimationEditorInspector.Width, height);

            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.textColor = new Color(0, 0, 0, 0.5f);
            labelStyle.fontSize = 10;

            float seconds = 0;
            float milliseconds = 0;

            for (int i = 0; i < SegmentsCount; i++)
            {
                float sizex = SegmentWidth;
                float sizey = height;

                float x = positionX + i * sizex;
                float y = positionY;

                bool expanded = false;

                if (sizex > 40)
                {
                    expanded = true;
                }
                else if (sizex > 30)
                {
                    expanded = i % 2 == 0;
                }
                else if (sizex > 20)
                {
                    expanded = i % 5 == 0;
                }
                else if (sizex > 10)
                {
                    expanded = i % 10 == 0;
                }
                else
                {
                    expanded = i % 20 == 0;
                }

                if (expanded)
                {
                    GUI.Label(new Rect(x + 3, y + height - 18, 100, height), seconds + ":" + Mathf.Round(milliseconds * 100).ToLeadingZerosString(2), labelStyle);

                    if (i > 0)
                    {
                        InternalUiAnimationEditorGUI.DrawTexture(x, y + sizey - 10, 1, 10, InternalUiAnimationEditorTextures.ColorBlack100);
                    }
                }
                else
                {
                    InternalUiAnimationEditorGUI.DrawTexture(x, y + sizey - 4, 1, 4, InternalUiAnimationEditorTextures.ColorBlack100);
                }

                milliseconds += 0.1f;

                while (milliseconds >= 1)
                {
                    milliseconds -= 1;
                    seconds += 1;
                }
            }
        }

        private static void RenderMenu(float positionX, float positionY, float width, float height)
        {
            GUIStyle splitStyle = new GUIStyle();
            splitStyle.fixedWidth = 1;
            splitStyle.fixedHeight = height;
            splitStyle.margin = new RectOffset(0, 0, 0, 0);
            splitStyle.normal.background = InternalUiAnimationEditorTextures.ColorBlack20;

            GUIStyle dropdownStyle = new GUIStyle(new GUIStyle(GUI.skin.GetStyle("Popup")));
            dropdownStyle.fixedHeight = height;
            dropdownStyle.margin = new RectOffset(0, 0, 0, 0);
            dropdownStyle.normal.background = InternalUiAnimationEditorTextures.ColorInvisible;
            dropdownStyle.hover.background = InternalUiAnimationEditorTextures.ColorBlack10;
            dropdownStyle.active.background = InternalUiAnimationEditorTextures.ColorBlack20;
            dropdownStyle.focused.background = InternalUiAnimationEditorTextures.ColorInvisible;

            GUIStyle buttonStyle = new GUIStyle(new GUIStyle(GUI.skin.GetStyle("Button")));
            buttonStyle.fixedWidth = height;
            buttonStyle.fixedHeight = height;
            buttonStyle.normal.background = InternalUiAnimationEditorTextures.ColorInvisible;
            buttonStyle.hover.background = InternalUiAnimationEditorTextures.ColorBlack10;
            buttonStyle.active.background = InternalUiAnimationEditorTextures.ColorBlack20;
            buttonStyle.margin = new RectOffset(0, 0, 0, 0);
            buttonStyle.padding = new RectOffset(0, 0, 0, 0);

            GUILayout.BeginArea(new Rect(positionX, positionY, width - 0, height), new GUIStyle());
            GUILayout.BeginHorizontal();

            // Play button
            if (InternalUiAnimationEditorPlayer.Activated)
            {
                Texture2D icon = new Texture2D(8, 8);
                Color color = new Color(0.22f, 0.22f, 0.22f, 1);
                for (int x = 0; x < icon.width; x++)
                {
                    for (int y = 0; y < icon.height; y++)
                    {
                        icon.SetPixel(x, y, color);
                    }
                }
                icon.Apply();

                InternalUiAnimationEditorGUI.Button
                (
                    icon,
                    buttonStyle,
                    () =>
                    {
                        InternalUiAnimationEditorPlayer.Stop();
                    }
                );
                GUILayout.Box(string.Empty, splitStyle);
            }
            else
            {
                InternalUiAnimationEditorGUI.Button
                (
                    EditorGUIUtility.IconContent("Animation.Play").image,
                    buttonStyle,
                    () =>
                    {
                        InternalUiAnimationEditorPlayer.Play();
                    }
                );
                GUILayout.Box(string.Empty, splitStyle);
            }

            // Active timeline popup
            InternalUiAnimationEditorGUI.Popup
            (
                InternalUiAnimationEditorSelection.TargetAnimationClipIndex,
                InternalUiAnimationEditorSelection.GetAnimationClipsNames(),
                dropdownStyle,
                (int value) =>
                {
                    GUI.FocusControl(string.Empty);
                    InternalUiAnimationEditorSelection.SetTargetAnimationClipIndex(value);
                }
            );

            GUILayout.Box(string.Empty, splitStyle);

            // Add timeline button
            InternalUiAnimationEditorGUI.Button
            (
                EditorGUIUtility.IconContent("Toolbar Plus").image,
                buttonStyle,
                () =>
                {
                    if (InternalUiAnimationEditorSelection.TargetAnimation != null)
                    {
                        UiAnimationClip timeline = new UiAnimationClip();
                        timeline.Name = "New Timeline " + InternalUiAnimationEditorSelection.TargetAnimation.AnimationClips.Count;

                        InternalUiAnimationEditorSelection.TargetAnimation.AnimationClips.Add(timeline);
                        InternalUiAnimationEditorSelection.SetTargetAnimationClipIndex(InternalUiAnimationEditorSelection.TargetAnimation.AnimationClips.IndexOf(timeline));
                    }
                }
            );
            GUILayout.Box(string.Empty, splitStyle);

            // Remove timeline button
            InternalUiAnimationEditorGUI.Button
            (
                EditorGUIUtility.IconContent("Toolbar Minus").image,
                buttonStyle,
                () =>
                {
                    if (InternalUiAnimationEditorSelection.TargetAnimation != null)
                    {
                        if (InternalUiAnimationEditorSelection.TargetAnimation.AnimationClips.Count > 1)
                        {
                            if (EditorUtility.DisplayDialog("Error", "Are you sure you want to delete " + InternalUiAnimationEditorSelection.TargetAnimationClip.Name + "?", "Yes", "No"))
                            {
                                InternalUiAnimationEditorSelection.TargetAnimation.AnimationClips.RemoveAt(InternalUiAnimationEditorSelection.TargetAnimationClipIndex);
                                InternalUiAnimationEditorSelection.SetTargetAnimationClipIndex(0);
                            }
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "You can not delete last timeline.", "OK");
                        }
                    }
                }
            );
            GUILayout.Box(string.Empty, splitStyle);

            // Dropdown icon
            Texture dropdownIcon = EditorGUIUtility.IconContent("Icon Dropdown").image;
            InternalUiAnimationEditorGUI.DrawTexture(width - 60, positionY + 9, dropdownIcon.width, dropdownIcon.height, dropdownIcon);

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}
#endif
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal
{
    public class InternalUiAnimationEditorCanvasItem
    {
        public UiAnimationClipSegment Target;

        public float Duration
        {
            get
            {
                return Target.Duration;
            }

            private set
            {
                Target.Duration = value;
            }
        }

        public float Delay
        {
            get
            {
                return Target.Delay;
            }

            private set
            {
                Target.Delay = value;
            }
        }

        public float Layer
        {
            get
            {
                return Target.Layer;
            }

            private set
            {
                Target.Layer = value;
            }
        }

        public float X
        {
            get
            {
                return Delay * InternalUiAnimationEditorTimeline.SegmentWidth * 10 + InternalUiAnimationEditorInspector.Width;
            }
        }

        public float Y
        {
            get
            {
                return Layer * Height + InternalUiAnimationEditorTimeline.Height;
            }
        }

        public float Width
        {
            get
            {
                return Duration * InternalUiAnimationEditorTimeline.SegmentWidth * 10;
            }
        }

        public float Height
        {
            get
            {
                return InternalUiAnimationEditorCanvas.LayerHeight;
            }
        }

        public bool IsCollided = false;

        public float CachedY;
        public float CachedX;

        /// <summary>
        /// Initializes a new instance of the InternalUiTweenerEditorCanvasItem.
        /// </summary>
        /// <param name="target">Target UiTweener.TimelineItem</param>
        public InternalUiAnimationEditorCanvasItem(UiAnimationClipSegment target)
        {
            Target = target;
        }

        /// <summary>
        /// Sets the delay.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetDelay(float value)
        {
            if (value < 0)
            {
                value = 0;
            }

            Delay = value;
        }

        /// <summary>
        /// Sets the duration.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetDuration(float value)
        {
            if (value < 0.1f)
            {
                value = 0.1f;
            }

            Duration = value;
        }

        /// <summary>
        /// Sets the layer.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetLayer(int value)
        {
            if (value < 0)
            {
                value = 0;
            }

            Layer = value;
        }

        /// <summary>
        /// Is the specified point inside of item bounds.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool HitTest(float x, float y)
        {
            bool hittest = x < X || x > X + Width - 1 || y < Y || y > Y + Height - 1;
            return !hittest;
        }

        public void Render()
        {
            if (Target.Predefined && Delay > 0)
            {
                float size = Delay * InternalUiAnimationEditorTimeline.SegmentWidth * 10;

                InternalUiAnimationEditorGUI.DrawTexture
                (
                    X - size,
                    Y,
                    size,
                    Height,
                    InternalUiAnimationEditorTextures.GenerateCanvasItemBackground
                    (
                        (int)size,
                        (int)Height,
                        Target.ItemType,
                        Target.EasingType,
                        true
                    )
                );
            }

            InternalUiAnimationEditorGUI.DrawTexture(X, Y, Width, Height, InternalUiAnimationEditorTextures.GenerateCanvasItemBackground((int)Width, (int)Height, Target.ItemType, Target.EasingType));

            if (InternalUiAnimationEditorSelection.CanvasItemToEdit == this)
            {
                InternalUiAnimationEditorGUI.DrawTexture(X, Y, Width, Height, InternalUiAnimationEditorTextures.ColorWhite20);
            }

            GUILayout.BeginArea(new Rect(Mathf.Round(X) + 5, Mathf.Round(Y) + 3, Mathf.Round(Width) - 10, Mathf.Round(Height) - 3));

            if (Target != null && Target.GameObject != null)
            {
                GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));

                if (IsCollided)
                {
                    labelStyle.normal.textColor = Color.red;
                    //IsCollided = false;
                }

                GUILayout.Label(Target.GameObject.name + " " + Target.ItemType.ToString(), labelStyle);
            }
            else
            {
                GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));

                labelStyle.normal.textColor = Color.red;

                GUILayout.Label("Missed", labelStyle);
            }

            GUILayout.EndArea();
        }

        public void SetCursor()
        {
            EditorGUIUtility.AddCursorRect(new Rect(X, Y, Width - 10, Height), MouseCursor.MoveArrow);
            EditorGUIUtility.AddCursorRect(new Rect(X + Width - 10, Y, 9, Height), MouseCursor.ResizeHorizontal);
        }
    }
}
#endif
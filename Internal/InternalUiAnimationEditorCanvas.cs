#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal
{
    public class InternalUiAnimationEditorCanvas
    {
        public static int Width
        {
            get
            {
                return (int)(InternalUiAnimationEditor.WindowWidth - InternalUiAnimationEditorInspector.Width);
            }
        }

        public static float TotalTime
        {
            get
            {
                float output = 0;

                for (int i = 0; i < Items.Count; i++)
                {
                    output = Mathf.Max(Items[i].Delay + Items[i].Duration, output);
                }

                return output;
            }
        }

        public static float LayerHeight = 40;

        public static List<GameObject> DragAndDropQueue = null;
        public static List<InternalUiAnimationEditorCanvasItem> Items = new List<InternalUiAnimationEditorCanvasItem>();

        private static UiAnimationClipSegment justAddedTween = null;

        public static float GetTimeByMousePosition(float position)
        {
            return GetTimeByLocalPosition(position - InternalUiAnimationEditorInspector.Width);
        }

        public static float GetTimeByLocalPosition(float position)
        {
            return Mathf.Floor(position / InternalUiAnimationEditorTimeline.SegmentWidth) / 10f;
        }

        public static int GetLayerByMousePosition(float position)
        {
            return GetLayerByLocalPosition(position - InternalUiAnimationEditorTimeline.Height);
        }

        public static int GetLayerByLocalPosition(float position)
        {
            return Mathf.FloorToInt(position / LayerHeight);
        }

        public static InternalUiAnimationEditorCanvasItem GetItemAt(float x, float y)
        {
            InternalUiAnimationEditorCanvasItem output = null;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].HitTest(x, y))
                {
                    output = Items[i];
                }
            }

            return output;
        }

        public static void Update()
        {
            if (DragAndDropQueue != null)
            {
                for (int i = 0; i < DragAndDropQueue.Count; i++)
                {
                    UiAnimationClipSegment tween = new UiAnimationClipSegment();
                    tween.GameObject = DragAndDropQueue[i] as GameObject;
                    tween.Delay = GetTimeByMousePosition(InternalUiAnimationEditor.MousePosition.x);
                    tween.Layer = GetLayerByMousePosition(InternalUiAnimationEditor.MousePosition.y);

                    RectTransform rectTransform = tween.GameObject.GetComponent<RectTransform>();

                    if (rectTransform != null)
                    {
                        tween.PositionFrom = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
                        tween.PositionTo = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

                        tween.RotationFrom = rectTransform.localRotation.eulerAngles.z;
                        tween.RotationTo = rectTransform.localRotation.eulerAngles.z;

                        tween.ScaleFrom = new Vector2(rectTransform.localScale.x, rectTransform.localScale.y);
                        tween.ScaleTo = new Vector2(rectTransform.localScale.x, rectTransform.localScale.y);

                        tween.SizeFrom = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
                        tween.SizeTo = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
                    }

                    if (InternalUiAnimationEditorSelection.TargetAnimationClip != null)
                    {
                        InternalUiAnimationEditorSelection.TargetAnimationClip.Items.Add(tween);
                        InternalUiAnimationEditorSelection.TargetAnimationClip.Items.Sort((a, b) => (a.Delay.CompareTo(b.Delay)));

                        justAddedTween = tween;
                    }
                }

                DragAndDropQueue = null;
            }

            if (InternalUiAnimationEditorSelection.CanvasItemToEdit == null)
            {
                Items = new List<InternalUiAnimationEditorCanvasItem>();

                for (int i = 0; i < InternalUiAnimationEditorSelection.TargetAnimationClip.Items.Count; i++)
                {
                    InternalUiAnimationEditorCanvasItem canvasItem = new InternalUiAnimationEditorCanvasItem
                    (
                        InternalUiAnimationEditorSelection.TargetAnimationClip.Items[i]
                    );

                    Items.Add(canvasItem);

                    if (canvasItem.Target == justAddedTween)
                    {
                        InternalUiAnimationEditorSelection.SetCanvasItemToEdit(canvasItem);
                        justAddedTween = null;
                    }
                }
            }

            if (InternalUiAnimationEditorSelection.CanvasItemToEdit != null)
            {
                Items.Remove(InternalUiAnimationEditorSelection.CanvasItemToEdit);
                Items.Add(InternalUiAnimationEditorSelection.CanvasItemToEdit);
            }

            if (InternalUiAnimationEditorSelection.CanvasItemToDrag != null)
            {
                UpdateDrag(InternalUiAnimationEditorSelection.CanvasItemToDrag);
            }

            if (InternalUiAnimationEditorSelection.CanvasItemToResize != null)
            {
                UpdateResize(InternalUiAnimationEditorSelection.CanvasItemToResize);
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].IsCollided = false;

                for (int j = 0; j < Items.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (Items[i].Target.GameObject != Items[j].Target.GameObject)
                    {
                        continue;
                    }

                    if (Items[i].Target.ItemType != Items[j].Target.ItemType)
                    {
                        continue;
                    }

                    if (Items[i].HitTest(Items[j].X, Items[i].Y) || Items[j].HitTest(Items[i].X, Items[j].Y))
                    {
                        Items[i].IsCollided = true;
                        break;
                    }
                }
            }
        }

        private static void UpdateDrag(InternalUiAnimationEditorCanvasItem item)
        {
            float originalTime = GetTimeByLocalPosition(item.CachedX - InternalUiAnimationEditor.MousePositionPress.x);
            float cursorTime = GetTimeByMousePosition(InternalUiAnimationEditor.MousePosition.x);

            int cursorLayer = GetLayerByMousePosition(InternalUiAnimationEditor.MousePosition.y);

            item.SetDelay(originalTime + cursorTime + 0.1f);
            item.SetLayer(cursorLayer);
        }

        private static void UpdateResize(InternalUiAnimationEditorCanvasItem item)
        {
            float cursorTime = GetTimeByMousePosition(InternalUiAnimationEditor.MousePosition.x);

            item.SetDuration(cursorTime - item.Delay + 0.1f);
        }

        public static void OnEvent()
        {
            if (Event.current.type == EventType.DragUpdated)
            {
                if (InternalUiAnimationEditor.MousePosition.x > InternalUiAnimationEditorInspector.Width)
                {
                    InternalUiAnimationEditorSelection.SetCanvasItemToEdit(null);
                }
            }

            // Drag and drop game objects
            if (Event.current.type == EventType.DragExited)
            {
                if (InternalUiAnimationEditor.MousePosition.x > InternalUiAnimationEditorInspector.Width && InternalUiAnimationEditor.MousePosition.y > InternalUiAnimationEditorTimeline.Height)
                {
                    object[] objects = DragAndDrop.objectReferences;
                    DragAndDropQueue = new List<GameObject>();

                    for (int i = 0; i < objects.Length; i++)
                    {
                        if (objects[i] is GameObject)
                        {
                            DragAndDropQueue.Add(objects[i] as GameObject);
                        }
                    }

                    Event.current.Use();
                }
            }

            if (Event.current.type == EventType.MouseDown)
            {
                InternalUiAnimationEditorCanvasItem item = GetItemAt(InternalUiAnimationEditor.MousePosition.x, InternalUiAnimationEditor.MousePosition.y);

                if (item != null)
                {
                    if (InternalUiAnimationEditor.MousePosition.x > item.X + item.Width - 10 && InternalUiAnimationEditor.MousePosition.x < item.X + item.Width)
                    {
                        InternalUiAnimationEditorSelection.SetCanvasItemToEdit(item);
                        InternalUiAnimationEditorSelection.SetCanvasItemToResize(item);
                    }

                    else
                    {
                        InternalUiAnimationEditorSelection.SetCanvasItemToEdit(item);
                        InternalUiAnimationEditorSelection.SetCanvasItemToDrag(item);

                        item.CachedX = item.X;
                        item.CachedY = item.Y;
                    }
                }
            }

            if (Event.current.type == EventType.MouseUp)
            {
                InternalUiAnimationEditorSelection.SetCanvasItemToDrag(null);
                InternalUiAnimationEditorSelection.SetCanvasItemToResize(null);

                if (InternalUiAnimationEditor.MousePosition.x > InternalUiAnimationEditorInspector.Width)
                {
                    InternalUiAnimationEditorCanvasItem item = GetItemAt(InternalUiAnimationEditor.MousePosition.x, InternalUiAnimationEditor.MousePosition.y);

                    if (item != null)
                    {
                        if (InternalUiAnimationEditorSelection.CanvasItemToEdit == null)
                        {
                            InternalUiAnimationEditorSelection.SetCanvasItemToEdit(item);
                        }
                    }
                    else
                    {
                        InternalUiAnimationEditorSelection.SetCanvasItemToEdit(null);
                    }
                }
            }
        }

        public static void Render(float positionX, float positionY, float width, float height)
        {
            if (Items.Count <= 0)
            {
                RenderPlayMode();
                return;
            }

            RenderLayers(positionX, positionY, width, height);

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Render();

                if (InternalUiAnimationEditorSelection.CanvasItemToDrag == null && InternalUiAnimationEditorSelection.CanvasItemToResize == null)
                {
                    Items[i].SetCursor();
                }
            }

            if (InternalUiAnimationEditorSelection.CanvasItemToDrag != null)
            {
                EditorGUIUtility.AddCursorRect(new Rect(0, 0, InternalUiAnimationEditor.WindowWidth, InternalUiAnimationEditor.WindowHeight), MouseCursor.MoveArrow);
            }

            if (InternalUiAnimationEditorSelection.CanvasItemToResize != null)
            {
                EditorGUIUtility.AddCursorRect(new Rect(0, 0, InternalUiAnimationEditor.WindowWidth, InternalUiAnimationEditor.WindowHeight), MouseCursor.ResizeHorizontal);
            }

            if (InternalUiAnimationEditorPlayer.Activated)
            {
                float playerPosition = InternalUiAnimationEditorPlayer.Time * InternalUiAnimationEditorTimeline.SegmentWidth * 10;

                InternalUiAnimationEditorGUI.DrawTexture(positionX + playerPosition, positionY, 1, height, InternalUiAnimationEditorTextures.ColorBlack100);
            }

            if (InternalUiAnimationEditor.MouseDragFromScene)
            {
                if (InternalUiAnimationEditor.MousePosition.x > positionX && InternalUiAnimationEditor.MousePosition.y > positionY)
                {
                    RenderCursor(positionX, positionY, width, height);
                }
            }
        }

        private static void RenderLayers(float positionX, float positionY, float width, float height)
        {
            for (int i = 0; i < 30; i++)
            {
                if (i % 2 == 0)
                {
                    InternalUiAnimationEditorGUI.DrawTexture(positionX, positionY + i * LayerHeight, width, LayerHeight, InternalUiAnimationEditorTextures.ColorBlack10);
                }
                else
                {
                    InternalUiAnimationEditorGUI.DrawTexture(positionX, positionY + i * LayerHeight, width, LayerHeight, InternalUiAnimationEditorTextures.ColorBlack20);
                }
            }
        }

        private static void RenderCursor(float positionX, float positionY, float width, float height)
        {
            float x = GetTimeByMousePosition(InternalUiAnimationEditor.MousePosition.x);
            float y = GetLayerByMousePosition(InternalUiAnimationEditor.MousePosition.y);

            x = positionX + x * InternalUiAnimationEditorTimeline.SegmentWidth * 10;
            y = positionY + y * LayerHeight;

            float sizex = InternalUiAnimationEditorTimeline.SegmentWidth * 10 * 0.5f;
            float sizey = LayerHeight;

            InternalUiAnimationEditorGUI.DrawTexture(x, y, sizex, 1, InternalUiAnimationEditorTextures.ColorWhite100);
            InternalUiAnimationEditorGUI.DrawTexture(x, y + sizey - 1, sizex, 1, InternalUiAnimationEditorTextures.ColorWhite100);

            InternalUiAnimationEditorGUI.DrawTexture(x, y, 1, sizey, InternalUiAnimationEditorTextures.ColorWhite100);
            InternalUiAnimationEditorGUI.DrawTexture(x + sizex - 1, y, 1, sizey, InternalUiAnimationEditorTextures.ColorWhite100);
        }


        private static void RenderPlayMode()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            labelStyle.alignment = TextAnchor.UpperCenter;

            float width = InternalUiAnimationEditor.WindowWidth - InternalUiAnimationEditorInspector.Width;

            GUILayout.BeginArea(new Rect(InternalUiAnimationEditorInspector.Width + width / 2 / 2, (InternalUiAnimationEditor.WindowHeight - 20) / 2, width / 2, 20));
            GUILayout.Label("Drag and drop GameObjects from screne to here to create a new tweens", labelStyle);
            GUILayout.EndArea();
        }

    }
}
#endif
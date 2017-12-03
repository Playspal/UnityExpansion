#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal
{
    public class InternalUiAnimationEditor : EditorWindow
    {
        public static InternalUiAnimationEditor Instance;

        public static int WindowWidth
        {
            get
            {
                return Instance != null ? (int)(Instance.position.width) : 0;
            }
        }

        public static int WindowHeight
        {
            get
            {
                return Instance != null ? (int)(Instance.position.height) : 0;
            }
        }

        public static Vector2 MousePosition = Vector2.zero;
        public static Vector2 MousePositionPress = Vector2.zero;
        public static bool MouseDragFromScene = false;

        public void Update()
        {
            Instance = this;

            InternalUiAnimationEditorSelection.Update();

            if (InternalUiAnimationEditorSelection.TargetGameObject == null)
            {
                return;
            }

            if (InternalUiAnimationEditorSelection.TargetAnimation == null)
            {
                return;
            }

            InternalUiAnimationEditorPlayer.Update();
            InternalUiAnimationEditorTimeline.Update();
            InternalUiAnimationEditorCanvas.Update();

            Repaint();
        }

        private void OnLostFocus()
        {
            InternalUiAnimationEditorPlayer.Stop();
        }

        private void OnGUI()
        {
            if (Application.isPlaying)
            {
                RenderPlayMode();
                return;
            }

            if (InternalUiAnimationEditorSelection.TargetGameObject == null)
            {
                RenderNoGameObject();
                return;
            }

            if (InternalUiAnimationEditorSelection.TargetAnimation == null)
            {
                RenderNoComponent();
                return;
            }

            if (Event.current.type == EventType.Repaint)
            {
                MousePosition.x = Event.current.mousePosition.x;
                MousePosition.y = Event.current.mousePosition.y;
            }

            if (Event.current.type == EventType.MouseDown)
            {
                MousePositionPress.x = Event.current.mousePosition.x;
                MousePositionPress.y = Event.current.mousePosition.y;
            }

            if (Event.current.type == EventType.MouseUp)
            {
                //MousePositionPress = Vector2.zero;
                //GUI.FocusControl(string.Empty);
            }

            if (Event.current.type == EventType.DragUpdated)
            {
                MouseDragFromScene = true;
            }

            if (Event.current.type == EventType.DragExited)
            {
                MouseDragFromScene = false;
            }

            if (MousePosition.x > InternalUiAnimationEditorInspector.Width && MousePosition.y > InternalUiAnimationEditorTimeline.Height)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            }

            InternalUiAnimationEditorTextures.Setup();

            InternalUiAnimationEditorTimeline.OnEvent();
            InternalUiAnimationEditorInspector.OnEvent();
            InternalUiAnimationEditorCanvas.OnEvent();

            InternalUiAnimationEditorTimeline.Render(InternalUiAnimationEditorInspector.Width, 0, InternalUiAnimationEditorTimeline.Width, InternalUiAnimationEditorTimeline.Height);
            InternalUiAnimationEditorInspector.Render(0, InternalUiAnimationEditorTimeline.Height, InternalUiAnimationEditorInspector.Width, WindowHeight);
            InternalUiAnimationEditorCanvas.Render(InternalUiAnimationEditorInspector.Width, InternalUiAnimationEditorTimeline.Height, InternalUiAnimationEditorCanvas.Width, WindowHeight - InternalUiAnimationEditorTimeline.Height);
        }

        private void RenderPlayMode()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            labelStyle.alignment = TextAnchor.UpperCenter;

            GUILayout.BeginArea(new Rect(WindowWidth / 2 / 2, (WindowHeight - 20) / 2, WindowWidth / 2, 20));
            GUILayout.Label("Editing UiTweener is not allowed in Play Mode", labelStyle);
            GUILayout.EndArea();
        }

        private void RenderNoGameObject()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            labelStyle.alignment = TextAnchor.UpperCenter;

            GUILayout.BeginArea(new Rect(WindowWidth / 2 / 2, (WindowHeight - 20) / 2, WindowWidth / 2, 20));
            GUILayout.Label("No GameObject is selected", labelStyle);
            GUILayout.EndArea();
        }

        private void RenderNoComponent()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            labelStyle.alignment = TextAnchor.UpperCenter;

            GUILayout.BeginArea(new Rect(WindowWidth / 2 / 2, (WindowHeight - 50) / 2, WindowWidth / 2, 50));
            GUILayout.Label("To begin animating " + InternalUiAnimationEditorSelection.TargetGameObject.name + ", create UiTweener.", labelStyle);
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect((WindowWidth - 120) / 2, (WindowHeight - 50) / 2 + 20, 120, 50));
            InternalUiAnimationEditorGUI.Button
            (
                "Create",
                () =>
                {
                    InternalUiAnimationEditorSelection.TargetGameObject.AddComponent<UiAnimation>();
                }
            );
            GUILayout.EndArea();
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        [MenuItem("Window/UiAnimation Editor")]
        public static void ShowWindow()
        {
            Instance = GetWindow(typeof(InternalUiAnimationEditor)) as InternalUiAnimationEditor;
            Instance.titleContent = new GUIContent("UiAnimation");

            Selection.selectionChanged += () =>
            {
                Instance.Update();
                Instance.Repaint();
            };
        }
    }
}
#endif
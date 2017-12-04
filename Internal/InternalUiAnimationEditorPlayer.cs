#if UNITY_EDITOR
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityExpansion.UI.Animation;

namespace UnityExpansionInternal
{
    public class InternalUiAnimationEditorPlayer
    {
        private class GameObjectState
        {
            public GameObject Target;

            // To cancel alpha tween
            public bool CanvasGroupExists;
            public float CanvasGroupAlpha;

            // To revert color tween
            public bool GraphicExists;
            public Color GraphicColor;

            // To cancel position tween
            public bool RectTransformExists;
            public Vector2 RectTransformPosition;

            // To cancel rotation tween
            public Vector3 RectTransformRotation;

            // To revert scale tween
            public Vector3 RectTransformScale;

            public void Apply(GameObject target)
            {
                Target = target;

                if (Target == null)
                {
                    return;
                }

                CanvasGroup canvasGroup = Target.GetComponent<CanvasGroup>();

                if (canvasGroup != null)
                {
                    CanvasGroupExists = true;
                    CanvasGroupAlpha = canvasGroup.alpha;
                }

                Graphic graphic = Target.GetComponent<Graphic>();

                if (graphic != null)
                {
                    GraphicExists = true;
                    GraphicColor = graphic.color;
                }

                RectTransform rectTransform = Target.GetComponent<RectTransform>();

                if (rectTransform != null)
                {
                    RectTransformExists = true;
                    RectTransformPosition = rectTransform.anchoredPosition;
                    RectTransformRotation = rectTransform.localRotation.eulerAngles;
                    RectTransformScale = rectTransform.localScale;
                }
            }

            public void Revert()
            {
                if(Target == null)
                {
                    return;
                }

                if (CanvasGroupExists)
                {
                    CanvasGroup canvasGroup = Target.GetComponent<CanvasGroup>();
                    canvasGroup.alpha = CanvasGroupAlpha;
                }
                else
                {
                    CanvasGroup canvasGroup = Target.GetComponent<CanvasGroup>();
                    if (canvasGroup != null)
                    {
                        GameObject.DestroyImmediate(canvasGroup);
                    }
                }

                if (GraphicExists)
                {
                    Graphic graphic = Target.GetComponent<Graphic>();
                    graphic.color = GraphicColor;
                }

                if (RectTransformExists)
                {
                    RectTransform rectTransform = Target.GetComponent<RectTransform>();

                    rectTransform.anchoredPosition = RectTransformPosition;
                    rectTransform.localRotation = Quaternion.Euler(RectTransformRotation);
                    rectTransform.localScale = RectTransformScale;
                }
            }
        }

        /// <summary>
        /// Is currerntly playing.
        /// </summary>
        public static bool Activated { get; private set; }

        /// <summary>
        /// Current playback time in seconds.
        /// </summary>
        public static float Time { get; private set; }

        // Realtime on last update. Used to calculate frame time.
        private static float _realtimeOnLastUpdate = 0;

        // List of objects that must be reverted.
        private static List<GameObjectState> _gameObjectsToRevert = new List<GameObjectState>();

        public static void Play()
        {
            _gameObjectsToRevert = new List<GameObjectState>();

            for (int i = 0; i < InternalUiAnimationEditorCanvas.Items.Count; i++)
            {
                if (InternalUiAnimationEditorCanvas.Items[i] == null)
                {
                    continue;
                }

                if (InternalUiAnimationEditorCanvas.Items[i].Target == null)
                {
                    continue;
                }

                if (InternalUiAnimationEditorCanvas.Items[i].Target.GameObject == null)
                {
                    continue;
                }

                GameObjectState state = new GameObjectState();
                state.Apply(InternalUiAnimationEditorCanvas.Items[i].Target.GameObject);

                _gameObjectsToRevert.Add(state);
            }

            Activated = true;
            Time = 0;
        }

        public static void Stop()
        {
            for (int i = 0; i < _gameObjectsToRevert.Count; i++)
            {
                _gameObjectsToRevert[i].Revert();
            }

            Activated = false;
            Time = 0;
        }

        public static void Update()
        {
            float delta = UnityEngine.Time.realtimeSinceStartup - _realtimeOnLastUpdate;
            _realtimeOnLastUpdate = UnityEngine.Time.realtimeSinceStartup;

            if (Activated)
            {
                Time += delta;

                if (Time > InternalUiAnimationEditorCanvas.TotalTime)
                {
                    Stop();
                    Play();
                }

                else
                {
                    Goto(Time);
                }
            }
        }

        private static void Goto(float position)
        {
            if (InternalUiAnimationEditorSelection.TargetAnimation == null)
            {
                return;
            }

            if (InternalUiAnimationEditorSelection.TargetAnimationClip == null)
            {
                return;
            }

            InternalUiAnimationEditorSelection.TargetAnimationClip.Rewind(position);
        }
    }
}
#endif
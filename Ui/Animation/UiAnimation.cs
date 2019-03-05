using System;
using System.Collections.Generic;

using UnityEngine;
using UnityExpansion.Services;

using UnityExpansion.UI.Layout;

namespace UnityExpansion.UI.Animation
{
    [Serializable]
    [DisallowMultipleComponent]
    [AddComponentMenu("Expansion/UiAnimation", 1)]
    public class UiAnimation : MonoBehaviour
    {
        /// <summary>
        /// List of attached animation clips.
        /// </summary>
        [SerializeField]
        public List<UiAnimationClip> AnimationClips = new List<UiAnimationClip>
        {
            new UiAnimationClip()
        };

        /// <summary>
        /// Invokes after animation is finished or stopped.
        /// </summary>
        public event Action<UiAnimationClip> OnComplete;

        /// <summary>
        /// Is the animation clip currently playing.
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// Current playback time in seconds.
        /// </summary>
        public float Time { get; private set; }

        // Currently playing animation.
        private UiAnimationClip _activeAnimation = null;

        /// <summary>
        /// Searches animation clip with specified name or id.
        /// </summary>
        /// <param name="clipNameOrID">The name or id of animation clip</param>
        /// <returns>Animation clip or null if not found</returns>
        public UiAnimationClip GetAnimationClip(string clipNameOrID)
        {
            return AnimationClips.Find(x => x.Name == clipNameOrID || x.ID == clipNameOrID);
        }

        /// <summary>
        /// Plays the animation clip with specified name or id from the beginning.
        /// </summary>
        /// <param name="nameOrID">The name or id of animation clip</param>
        public void Play(string clipNameOrID)
        {
            UiAnimationClip clip = GetAnimationClip(clipNameOrID);

            if (clip != null)
            {
                Play(clip);
            }
        }

        /// <summary>
        /// Plays the specified animation clip from the beginning.
        /// </summary>
        /// <param name="animation">Animation clip</param>
        public void Play(UiAnimationClip animation)
        {
            if (IsPlaying)
            {
                Stop();
            }

            _activeAnimation = animation;

            if (_activeAnimation != null)
            {
                IsPlaying = true;
                Time = 0;

                _activeAnimation.RewindToBegin();
            }
        }

        /// <summary>
        /// Stops all playing animation clips that were started with this UiAnimation.
        /// </summary>
        public void Stop(bool gotoLastFrame = true)
        {
            if (!IsPlaying || _activeAnimation == null)
            {
                return;
            }

            if (gotoLastFrame)
            {
                _activeAnimation.RewindToEnd();
            }

            OnComplete.InvokeIfNotNull(_activeAnimation);

            IsPlaying = false;
            Time = 0;

            _activeAnimation = null;
        }

        // Initialization
        private void Awake()
        {
            for (int i = 0; i < AnimationClips.Count; i++)
            {
                AnimationClips[i].SetAnimationController(this);
            }
        }

        // Main iteration
        private void Update()
        {
            if(IsPlaying)
            {
                Time += UnityEngine.Time.deltaTime;

                if(_activeAnimation != null)
                {
                    _activeAnimation.Rewind(Time);

                    if (Time > _activeAnimation.GetDuration())
                    {
                        if (_activeAnimation.Loop)
                        {
                            Time = 0;
                        }
                        else
                        {
                            Stop();
                        }
                    }
                }
            }
        }
        
        // Initialization
        private void Start()
        {
            for(int i = 0; i < AnimationClips.Count; i++)
            {
                if(AnimationClips[i].PlayOnAwake)
                {
                    Play(AnimationClips[i]);
                }
            }

            UiLayoutElement element = FindParentLayoutElement(gameObject.transform);

            if(element != null)
            {
                element.OnShow += OnParentLayoutElementShowBegin;
                element.OnHide += OnParentLayoutElementHideBegin;
            }
        }

        // Parent layout element show begin handler
        private void OnParentLayoutElementShowBegin()
        {
            for (int i = 0; i < AnimationClips.Count; i++)
            {
                if (AnimationClips[i].PlayOnLayoutElementShow)
                {
                    Play(AnimationClips[i]);
                }
            }
        }

        // Parent layout element hide begin handler.
        private void OnParentLayoutElementHideBegin()
        {
            for (int i = 0; i < AnimationClips.Count; i++)
            {
                if (AnimationClips[i].PlayOnLayoutElementHide)
                {
                    Play(AnimationClips[i]);
                }
            }
        }


        // Recursively searches parent layout element.
        private UiLayoutElement FindParentLayoutElement(Transform transform)
        {
            UiLayoutElement element = transform.GetComponent<UiLayoutElement>();

            if(element != null)
            {
                return element;
            }

            if(transform.parent == null)
            {
                return null;
            }

            return FindParentLayoutElement(transform.parent);
        }
    }
}
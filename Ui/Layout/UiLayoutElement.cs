﻿using System;

using UnityEngine;
using UnityExpansion.UI.Animation;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Base ui layout element. Provides functionality to play show and hide animation clips.
    /// </summary>
    /// <seealso cref="UnityExpansion.UI.UiObject" />
    public class UiLayoutElement : UiObject
    {
        /// <summary>
        /// Name of show animation clip
        /// </summary>
        [HideInInspector]
        public string AnimationShow = string.Empty;

        /// <summary>
        /// Name of hide animation clip
        /// </summary>
        [HideInInspector]
        public string AnimationHide = string.Empty;

        /// <summary>
        /// Invokes right after element show begin.
        /// </summary>
        public event Action OnShowBegin;

        /// <summary>
        /// Invokes after element is shown.
        /// If element have show animation, OnShow will be invoked after animation will be played.
        /// </summary>
        public event Action OnShow;

        /// <summary>
        /// Invokes right after element hide begin.
        /// </summary>
        public event Action OnHideBegin;

        /// <summary>
        /// Invokes after element is hiden.
        /// If element have hide animation, OnHide will be invoked after animation will be played.
        /// </summary>
        public event Action OnHide;

        // Current visibility. Sets to true right before show animations and sets to false before hide animations
        // Used to prevent start animation if it is already started
        private bool _isScreenShown = false;

        // Attached UiAnimation component
        private UiAnimation _animation;

        // Attached animation clips
        private UiAnimationClip _animationShow;
        private UiAnimationClip _animationHide;

        /// <summary>
        /// MonoBehavior Awake handler.
        /// In inherited classes always use base.Awake() when overriding this method.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            _animation = GetComponent<UiAnimation>();

            if (_animation != null)
            {
                _animation.OnComplete += OnAnimationCompleted;

                _animationShow = _animation.GetAnimationClipByName(AnimationShow);
                _animationHide = _animation.GetAnimationClipByName(AnimationHide);

                if (_animationShow != null)
                {
                    _animationShow.PlayOnAwake = false;
                    _animationShow.PlayOnLayoutElementShow = false;
                    _animationShow.PlayOnLayoutElementHide = false;
                    _animationShow.Loop = false;
                }

                if (_animationHide != null)
                {
                    _animationHide.PlayOnAwake = false;
                    _animationHide.PlayOnLayoutElementShow = false;
                    _animationHide.PlayOnLayoutElementHide = false;
                    _animationHide.Loop = false;
                }
            }
        }

        /// <summary>
        /// Gets the duration of the show animation.
        /// </summary>
        /// <returns>Time in seconds</returns>
        public float GetAnimationShowDuration()
        {
            return _animationShow != null ? _animationShow.GetDuration() : 0;
        }

        /// <summary>
        /// Gets the duration of the hide animation.
        /// </summary>
        /// <returns>Time in seconds</returns>
        public float GetAnimationHideDuration()
        {
            return _animationHide != null ? _animationHide.GetDuration() : 0;
        }

        /// <summary>
        /// Shows this element.
        /// If element have child tweens that presents show animation, they will be played.
        /// </summary>
        public void Show()
        {
            if (IsDestroyed || _isScreenShown)
            {
                return;
            }

            ShowBegin();
            StopAnimation();

            SetActive(true);

            if (GetAnimationShowDuration() > 0)
            {
                _animation.Play(_animationShow.Name);
            }
            else
            {
                ShowEnd();
            }
        }

        /// <summary>
        /// Shows element immediately without playing any animations.
        /// If element have child tweens that presents show animation, they will immediately animated to last frame.
        /// </summary>
        public void ShowImmediately()
        {
            if (IsDestroyed)
            {
                return;
            }

            ShowBegin();
            StopAnimation();

            SetActive(true);

            if (GetAnimationShowDuration() == 0)
            {
                ShowEnd();
            }
        }

        /// <summary>
        /// Hides this element.
        /// If element have child tweens that presents hide animation, they will be played.
        /// </summary>
        public void Hide()
        {
            if (IsDestroyed || !_isScreenShown)
            {
                return;
            }

            HideBegin();
            StopAnimation();

            if (GetAnimationHideDuration() > 0)
            {
                _animation.Play(_animationHide.Name);
            }
            else
            {
                HideEnd();
            }
        }

        /// <summary>
        /// Hides element immediately without playing any animations.
        /// </summary>
        public void HideImmediately()
        {
            if (IsDestroyed)
            {
                return;
            }

            HideBegin();
            StopAnimation();

            if (GetAnimationHideDuration() == 0)
            {
                HideEnd();
            }
        }

        // Stops animation in case it exists.
        // If there was playing animation OnAnimationCompleted will be invoked.
        private void StopAnimation()
        {
            if (_animation != null)
            {
                _animation.Stop(true);
            }
        }

        // Animation completed handler.
        private void OnAnimationCompleted(UiAnimationClip animation)
        {
            if (animation == _animationShow)
            {
                ShowEnd();
            }

            if (animation == _animationHide)
            {
                HideEnd();
            }
        }

        /// <summary>
        /// Performed when Show processes started.
        /// In inherited classes always use base.ShowBegin() when overriding this method.
        /// </summary>
        protected virtual void ShowBegin()
        {
            OnShowBegin.InvokeIfNotNull();

            _isScreenShown = true;
        }

        /// <summary>
        /// Performed when Show processes completed.
        /// In inherited classes always use base.ShowEnd() when overriding this method.
        /// </summary>
        protected virtual void ShowEnd()
        {
            OnShow.InvokeIfNotNull();
        }

        /// <summary>
        /// Performed when Hide processes started.
        /// In inherited classes always use base.HideBegin() when overriding this method.
        /// </summary>
        protected virtual void HideBegin()
        {
            OnHideBegin.InvokeIfNotNull();

            _isScreenShown = false;
        }

        /// <summary>
        /// Performed when Hide processes completed.
        /// In inherited classes always use base.HideEnd() when overriding this method.
        /// </summary>
        protected virtual void HideEnd()
        {
            OnHide.InvokeIfNotNull();

            SetActive(false);
        }
    }
}
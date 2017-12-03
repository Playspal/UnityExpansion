using System;

using UnityEngine;
using UnityEngine.UI;
using UnityExpansion.Services;
using UnityExpansion.Utilities;

namespace UnityExpansion.UI
{
    [AddComponentMenu("Expansion/Ui/Button", 1)]
    public class UiButton : UiObject
    {
        public enum TransitionType
        {
            None,
            ObjectsSwap
        }

        public enum State
        {
            Normal,
            Hover,
            Pressed,
            Disabled
        }

        [SerializeField]
        internal TransitionType _transitionType = TransitionType.None;

        [SerializeField]
        internal float _transitionTime = 0.1f;

        [SerializeField]
        internal GameObject _objectNormal;

        [SerializeField]
        internal GameObject _objectHover;

        [SerializeField]
        internal GameObject _objectPressed;

        [SerializeField]
        internal GameObject _objectDisabled;

        [SerializeField]
        internal Text[] _captions = new Text[0];

        [SerializeField]
        internal float _interactionDelay = 0f;

        [SerializeField]
        internal string _signalClick = "";

        [SerializeField]
        internal string _signalPress = "";

        [SerializeField]
        internal string _signalRelease = "";

        private State _state = State.Normal;

        private float _interactionTime = 0;

        /// <summary>
        /// Called on button click.
        /// </summary>
        public event Action OnClick;

        /// <summary>
        /// Called on button press
        /// </summary>
        public event Action OnPress;

        /// <summary>
        /// Called on button release.
        /// </summary>
        public event Action OnRelease;

        /// <summary>
        /// Called on cursor rolled over the button.
        /// </summary>
        public event Action OnMouseOver;

        /// <summary>
        /// Called on cursor rolled out the button.
        /// </summary>
        public event Action OnMouseOut;

        /// <summary>
        /// Is button enabled and interactable.
        /// </summary>
        public bool IsEnabled = true;

        /// <summary>
        /// Is button currently pressed.
        /// </summary>
        public bool IsPressed = false;

        /// <summary>
        /// The is mouse cursor currently on the button.
        /// </summary>
        public bool IsHovered = false;

        /// <summary>
        /// MonoBehavior Start handler.
        /// In inherited classes always use base.Start() when overriding this method.
        /// </summary>
        protected override void Start()
        {
            base.Start();

            switch (_transitionType)
            {
                case TransitionType.None:
                    break;

                case TransitionType.ObjectsSwap:
                    if (_objectNormal != null)
                    {
                        _objectNormal.SetActive(true);
                        _objectNormal.SetAlpha(_objectDisabled == null || IsEnabled ? 1 : 0);
                    }

                    if (_objectHover != null)
                    {
                        _objectHover.SetActive(true);
                        _objectHover.SetAlpha(0);
                    }

                    if (_objectPressed != null)
                    {
                        _objectPressed.SetActive(true);
                        _objectPressed.SetAlpha(0);
                    }

                    if (_objectDisabled != null)
                    {
                        _objectDisabled.SetActive(true);
                        _objectDisabled.SetAlpha(IsEnabled ? 0 : 1);
                    }
                    break;
            }

            UiEvents.AddMousePressListener(gameObject, OnPressHandler);
            UiEvents.AddMouseReleaseListener(gameObject, OnReleaseHandler);
            UiEvents.AddMouseOverListener(gameObject, OnMouseOverHandler);
            UiEvents.AddMouseOutListener(gameObject, OnMouseOutHandler);
        }

        /// <summary>
        /// MonoBehavior OnDisable handler.
        /// In inherited classes always use base.OnDisable() when overriding this method.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            OnMouseOutHandler();
        }

        /// <summary>
        /// Set text to all button's captions
        /// </summary>
        /// <param name="value">Text string</param>
        public void SetCaption(string value)
        {
            for (int i = 0; i < _captions.Length; i++)
            {
                if (_captions[i] != null)
                {
                    _captions[i].text = value;
                }
            }
        }

        // Mouse press handler
        private void OnPressHandler()
        {
            if (IsEnabled && Time.realtimeSinceStartup - _interactionTime > _interactionDelay)
            {
                IsPressed = true;

                OnPress.InvokeIfNotNull();

                if (!string.IsNullOrEmpty(_signalPress))
                {
                    Signals.Dispatch(_signalPress);
                }

                SetState(State.Pressed);
            }
        }

        // Mouse release handler
        private void OnReleaseHandler()
        {
            if (IsEnabled && Time.realtimeSinceStartup - _interactionTime > _interactionDelay)
            {
                OnRelease.InvokeIfNotNull();

                if (!string.IsNullOrEmpty(_signalRelease))
                {
                    Signals.Dispatch(_signalRelease);
                }

                if (IsPressed)
                {
                    IsPressed = false;

                    OnClick.InvokeIfNotNull();

                    if (!string.IsNullOrEmpty(_signalClick))
                    {
                        Signals.Dispatch(_signalClick);
                    }
                }

                SetState(IsHovered ? State.Hover : State.Normal);

                _interactionTime = Time.realtimeSinceStartup;
            }
        }

        // Mouse over handler
        private void OnMouseOverHandler()
        {
            if (IsEnabled && Time.realtimeSinceStartup - _interactionTime > _interactionDelay)
            {
                IsHovered = true;

                OnMouseOver.InvokeIfNotNull();

                SetState(State.Hover);
            }
        }

        // Mouse out handler
        private void OnMouseOutHandler()
        {
            if (IsEnabled)
            {
                IsHovered = false;

                if (IsPressed)
                {
                    OnReleaseHandler();
                }

                OnMouseOut.InvokeIfNotNull();

                SetState(State.Normal);
            }
        }

        // Set current state
        private void SetState(State state)
        {
            switch (_transitionType)
            {
                case TransitionType.None:
                    break;

                case TransitionType.ObjectsSwap:
                    if (state == State.Hover && _objectHover == null)
                    {
                        SetState(State.Normal);
                        return;
                    }

                    if (state == State.Pressed && _objectPressed == null)
                    {
                        return;
                    }
                    break;
            }

            _state = state;
        }

        /// <summary>
        /// MonoBehavior Update handler.
        /// In inherited classes always use base.Update() when overriding this method.
        /// </summary>
        protected override void Update()
        {
            base.Update();

            switch (_transitionType)
            {
                case TransitionType.None:
                    break;

                case TransitionType.ObjectsSwap:
                    switch (_state)
                    {
                        case State.Normal:
                            UpdateTransitionObjects(true, false, false, false);
                            break;

                        case State.Hover:
                            UpdateTransitionObjects(false, true, false, false);
                            break;

                        case State.Pressed:
                            UpdateTransitionObjects(false, false, true, false);
                            break;

                        case State.Disabled:
                            UpdateTransitionObjects(false, false, false, true);
                            break;
                    }
                    break;
            }
        }

        // Update transition for group of objects
        private void UpdateTransitionObjects(bool normal, bool hover, bool pressed, bool disabled)
        {
            if (normal && UpdateTransitionObject(_objectNormal, normal))
            {
                UpdateTransitionObject(_objectHover, false);
                UpdateTransitionObject(_objectPressed, false);
                UpdateTransitionObject(_objectDisabled, false);
            }

            if (hover && UpdateTransitionObject(_objectHover, hover))
            {
                UpdateTransitionObject(_objectNormal, false);
                UpdateTransitionObject(_objectPressed, false);
                UpdateTransitionObject(_objectDisabled, false);
            }

            if (pressed && UpdateTransitionObject(_objectPressed, pressed))
            {
                UpdateTransitionObject(_objectNormal, false);
                UpdateTransitionObject(_objectHover, false);
                UpdateTransitionObject(_objectDisabled, false);
            }

            if (disabled && UpdateTransitionObject(_objectDisabled, disabled))
            {
                UpdateTransitionObject(_objectNormal, false);
                UpdateTransitionObject(_objectHover, false);
                UpdateTransitionObject(_objectPressed, false);
            }
        }

        // Update transition for single object
        private bool UpdateTransitionObject(GameObject target, bool active)
        {
            if (target != null)
            {
                if (_transitionTime == 0)
                {
                    target.SetAlpha(active ? 1 : 0);
                }
                else
                {
                    if (active)
                    {
                        target.FadeIn(1 * UtilityTime.DeltaTime / _transitionTime);
                    }
                    else
                    {
                        target.FadeOut(1 * UtilityTime.DeltaTime / _transitionTime);
                    }
                }

                return target.GetAlpha() == 1;
            }

            return true;
        }
    }
}
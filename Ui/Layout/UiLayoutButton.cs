using System;
using UnityEngine;
using UnityExpansion.UI.Layout.Processor;

namespace UnityExpansion.UI.Layout
{
    [Serializable]
    public class UiLayoutButton : UiLayoutElement
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

        [UiLayoutProcessorEvent(Group = "Mouse events", Weight = 1)]
        public event Action OnMouseOver;

        [UiLayoutProcessorEvent(Group = "Mouse events", Weight = 2)]
        public event Action OnMouseOut;

        [UiLayoutProcessorEvent(Group = "Mouse events", Weight = 3)]
        public event Action OnMousePress;

        [UiLayoutProcessorEvent(Group = "Mouse events", Weight = 4)]
        public event Action OnMouseRelease;

        [UiLayoutProcessorEvent(Group = "Mouse events", Weight = 5)]
        public event Action OnMouseClick;

        /// <summary>
        /// Is the button enabled and interactable
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
        }

        [SerializeField]
        private TransitionType _transitionType = TransitionType.None;

        [SerializeField]
        private UiLayoutButtonTransition _transition;

        // Is the button enabled and interactable
        [SerializeField]
        private bool _isEnabled = true;

        [UiLayoutProcessorHandler(Group = "Mouse events", Weight = 1)]
        public void ButtonEnable()
        {
            SetTransition(UiLayoutButtonTransition.State.Normal);
            _isEnabled = true;
        }

        [UiLayoutProcessorHandler(Group = "Mouse events", Weight = 2)]
        public void ButtonDisable()
        {
            SetTransition(UiLayoutButtonTransition.State.Disabled);
            _isEnabled = false;
        }

        protected override void Awake()
        {
            base.Awake();

            UiEvents.AddMouseOverListener(gameObject, MouseOverHandler);
            UiEvents.AddMouseOutListener(gameObject, MouseOutHandler);
            UiEvents.AddMousePressListener(gameObject, MousePressHandler);
            UiEvents.AddMouseReleaseListener(gameObject, MouseReleaseHandler);
        }

        protected virtual void MouseOverHandler()
        {
            if (!IsEnabled)
            {
                return;
            }

            SetTransition(UiLayoutButtonTransition.State.Hover);
            OnMouseOver.InvokeIfNotNull();
        }

        protected virtual void MouseOutHandler()
        {
            if (!IsEnabled)
            {
                return;
            }

            SetTransition(UiLayoutButtonTransition.State.Normal);
            OnMouseOut.InvokeIfNotNull();
        }

        protected virtual void MousePressHandler()
        {
            if (!IsEnabled)
            {
                return;
            }

            SetTransition(UiLayoutButtonTransition.State.Pressed);
            OnMousePress.InvokeIfNotNull();
        }

        protected virtual void MouseReleaseHandler()
        {
            if (!IsEnabled)
            {
                return;
            }

            SetTransition(UiLayoutButtonTransition.State.Normal);

            OnMouseRelease.InvokeIfNotNull();
            OnMouseClick.InvokeIfNotNull();
        }

        private void SetTransition(UiLayoutButtonTransition.State state)
        {
            if (_transition != null)
            {
                _transition.SetState(state);
            }
        }
    }
}
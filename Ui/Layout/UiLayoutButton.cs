using System;
using UnityExpansion.UI.Layout.Processor;

namespace UnityExpansion.UI.Layout
{
    public class UiLayoutButton : UiLayoutElement
    {
        [UiLayoutProcessorEvent(Group = "Mouse events", Weight = 1)]
        public event Action OnMouseOver;

        [UiLayoutProcessorEvent(Group = "Mouse events", Weight = 2)]
        public event Action OnMouseOut;

        [UiLayoutProcessorEvent(Group = "Mouse events", Weight = 3)]
        public event Action OnMousePress;

        [UiLayoutProcessorEvent(Group = "Mouse events", Weight = 4)]
        public event Action OnMouseRelease;

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
            OnMouseOver.InvokeIfNotNull();
        }

        protected virtual void MouseOutHandler()
        {
            OnMouseOut.InvokeIfNotNull();
        }

        protected virtual void MousePressHandler()
        {
            OnMousePress.InvokeIfNotNull();
        }

        protected virtual void MouseReleaseHandler()
        {
            OnMouseRelease.InvokeIfNotNull();
        }
    }
}
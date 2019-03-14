using System;

using UnityEngine;

namespace UnityExpansion.UI.Layout
{
    [Serializable, HideInInspector]
    public class UiLayoutButtonTransition : MonoBehaviour
    {
        public enum State
        {
            Normal,
            Hover,
            Pressed,
            Disabled
        }

        /// <summary>
        /// Current state
        /// </summary>
        public State StateCurrent { get; private set; }

        /// <summary>
        /// Sets the state
        /// </summary>
        public virtual void SetState(State state)
        {
            StateCurrent = state;
        }

        protected virtual void Awake()
        {
            SetState(State.Normal);
        }

        protected virtual void Update()
        {
        }
    }
}
using System;

using UnityEngine;

namespace UnityExpansion.UI.Layout
{
    [Serializable]
    public class UiLayoutButtonTransition : ScriptableObject
    {
        public enum State
        {
            Normal,
            Hover,
            Pressed,
            Disabled
        }

        public State StateCurrent { get; private set; }

        public virtual void Initialization()
        {
            SetState(State.Normal);
        }

        public virtual void SetState(State state)
        {
            StateCurrent = state;
        }

        public virtual void Update()
        {
        }
    }
}
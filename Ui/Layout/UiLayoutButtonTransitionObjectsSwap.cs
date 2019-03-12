using System.Collections.Generic;

using UnityEngine;

namespace UnityExpansion.UI.Layout
{
    public class UiLayoutButtonTransitionObjectsSwap : UiLayoutButtonTransition
    {
        [SerializeField]
        private GameObject _objectNormal;

        [SerializeField]
        private GameObject _objectHover;

        [SerializeField]
        private GameObject _objectPressed;

        [SerializeField]
        private GameObject _objectDisabled;

        [SerializeField]
        private float _transitionTime = 0.1f;

        private Dictionary<State, GameObject> _objects = new Dictionary<State, GameObject>();

        public override void Initialization()
        {
            base.Initialization();

            SetupObject(State.Normal, _objectNormal);
            SetupObject(State.Hover, _objectHover);
            SetupObject(State.Pressed, _objectPressed);
            SetupObject(State.Disabled, _objectDisabled);
        }

        public override void Update()
        {
            base.Update();

            GameObject currentGameObject = _objects.ContainsKey(StateCurrent) ? _objects[StateCurrent] : null;

            if (currentGameObject != null)
            {
                if(currentGameObject.GetAlpha() < 1)
                {
                    currentGameObject.FadeIn(1 * Time.deltaTime / _transitionTime);
                }
                else
                {
                    foreach (KeyValuePair<State, GameObject> pair in _objects)
                    {
                        if (pair.Key != StateCurrent)
                        {
                            pair.Value.FadeOut(1 * Time.deltaTime / _transitionTime);
                        }
                    }
                }
            }
        }

        private void SetupObject(State state, GameObject target)
        {
            if(target != null)
            {
                _objects.Add(state, target);
            }
        }        
    }
}
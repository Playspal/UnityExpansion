using UnityEngine;
using UnityExpansion.Services;

namespace UnityExpansion.UI
{
    [RequireComponent(typeof(UiObject))]
    [AddComponentMenu("Expansion/UiSignal", 1)]
    public class UiSignal : MonoBehaviour
    {
        public enum Type
        {
            MouseOver,
            MouseOut,
            MousePress,
            MouseRelease,
            KeyPress,
            KeyRelease
        }

        [SerializeField]
        private Type _type = Type.MouseOver;

        [SerializeField]
        private KeyCode _keyCode = KeyCode.Escape;

        [SerializeField]
        internal string[] _signals = new string[0];

        private bool _isKeyPressListener = false;
        private bool _isKeyReleaseListener = false;

        private void Awake()
        {
            switch(_type)
            {
                case Type.MouseOut:
                    UiEvents.AddMouseOutListener(gameObject, Dispatch);
                    break;

                case Type.MouseOver:
                    UiEvents.AddMouseOverListener(gameObject, Dispatch);
                    break;

                case Type.MousePress:
                    UiEvents.AddMousePressListener(gameObject, Dispatch);
                    break;

                case Type.MouseRelease:
                    UiEvents.AddMouseReleaseListener(gameObject, Dispatch);
                    break;

                case Type.KeyPress:
                    _isKeyPressListener = true;
                    break;

                case Type.KeyRelease:
                    _isKeyReleaseListener = true;
                    break;
            }
        }

        private void Update()
        {
            if (_isKeyPressListener && Input.GetKeyDown(_keyCode))
            {
                Dispatch();
            }

            else if (_isKeyReleaseListener && Input.GetKeyUp(_keyCode))
            {
                Dispatch();
            }
        }

        private void Dispatch()
        {
            Signals.DispatchGroup(_signals);
        }
    }
}
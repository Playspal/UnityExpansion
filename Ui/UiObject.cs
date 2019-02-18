using UnityEngine;

namespace UnityExpansion.UI
{
    /// <summary>
    /// UnityExpansion.UI base object. Provides easy access to most used
    /// RectTransform properties such as X, Y, Width, Height, Rotation, ScaleX, ScaleY.
    /// But access to that properties is not so fast as direct access by RectTransform,
    /// so it's not reccomended to use that properties in case you need to update a lot of UiObjects on each frame.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Expansion/UiObject", 1)]
    public class UiObject : MonoBehaviour
    {


        /// <summary>
        /// Is game object destroyed.
        /// </summary>
        public bool IsDestroyed
        {
            get
            {
                // MonoBehaviour specific. Google it to learn more.
                return this == null;
            }
        }

        /// <summary>
        /// Is the GameObject active.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return gameObject != null ? gameObject.activeSelf : false;
            }
        }

        /// <summary>
        /// RectTransform.
        /// </summary>
        public RectTransform RectTransform
        {
            get
            {
                return gameObject.GetComponent<RectTransform>();
            }
        }

        /// <summary>
        /// RectTransform anchored position X.
        /// </summary>
        public float X
        {
            get
            {
                return RectTransform.anchoredPosition.x;
            }
            set
            {
                SetPosition(value, Y, false);
            }
        }

        /// <summary>
        /// RectTransform anchored position Y.
        /// </summary>
        public float Y
        {
            get
            {
                return RectTransform.anchoredPosition.y;
            }
            set
            {
                SetPosition(X, value, false);
            }
        }

        /// <summary>
        /// RectTransform width.
        /// </summary>
        public float Width
        {
            get
            {
                return RectTransform.rect.width;
            }
            set
            {
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
            }
        }

        /// <summary>
        /// RectTransform height.
        /// </summary>
        public float Height
        {
            get
            {
                return RectTransform.rect.height;
            }
            set
            {
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
            }
        }

        /// <summary>
        /// RectTransform local rotation by Z axis in degress.
        /// </summary>
        public float Rotation
        {
            get
            {
                return RectTransform.localRotation.eulerAngles.z;
            }
            set
            {
                Vector3 euler = RectTransform.localRotation.eulerAngles;
                RectTransform.localRotation = Quaternion.Euler(euler.x, euler.y, value);
            }
        }

        /// <summary>
        /// RectTransform horizontal local scale.
        /// </summary>
        public float ScaleX
        {
            get
            {
                return RectTransform.localScale.x;
            }
            set
            {
                RectTransform.localScale = new Vector2(value, ScaleY);
            }
        }

        /// <summary>
        /// RectTransform vertical local scale.
        /// </summary>
        public float ScaleY
        {
            get
            {
                return RectTransform.localScale.y;
            }
            set
            {
                RectTransform.localScale = new Vector2(ScaleX, value);
            }
        }

        /// <summary>
        /// Alpha value. Uses CanvasGroup component to set this property.
        /// </summary>
        public float Alpha
        {
            get
            {
                return gameObject.GetAlpha();
            }
            set
            {
                gameObject.SetAlpha(value);
            }
        }

        /// <summary>
        /// Destroys game object.
        /// </summary>
        public void Destroy()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Activates/Deactivates the UiObject and his GameObject.
        /// </summary>
        /// <param name="value">Activate or deactivation the object</param>
        public void SetActive(bool value)
        {
            if (value != IsActive)
            {
                gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Set the parent of the Graphics transform.
        /// </summary>
        /// <param name="parent">The parent Transform to use</param>
        /// <param name="worldPositionStays">If true, the parent-relative position, scale and rotation are modified such that the object keeps the same world space position, rotation and scale as before.</param>
        public void SetParent(RectTransform parent, bool worldPositionStays = false)
        {
            RectTransform.SetParent(parent, worldPositionStays);
        }

        /// <summary>
        /// Sets the Object's position.
        /// </summary>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        /// <param name="inScreenSpace">Is it Screen Space position or RectTransform anchoredPosition</param>
        public void SetPosition(float x, float y, bool inScreenSpace = false)
        {
            if (inScreenSpace)
            {
                RectTransform.SetPositionInScreenSpace(x, y);
            }
            else
            {
                RectTransform.anchoredPosition = new Vector2(x, y);
            }
        }

        /// <summary>
        /// Gets object's bound box.
        /// </summary>
        /// <param name="inScreenSpace">Is bounds in Screen Space or RectTransform.rect</param>
        /// <returns>Rect object</returns>
        public Rect GetBounds(bool inScreenSpace = false)
        {
            return inScreenSpace ? RectTransform.GetBoundsInScreenSpace() : RectTransform.rect;
        }

        /// <summary>
        /// Is the screen point inside of object's bounds box.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="inScreenSpace">Is coordinates in Screen Space or in local space</param>
        /// <returns>True if point inside of object.</returns>
        public bool HitTest(float x, float y, bool inScreenSpace = false)
        {
            Rect bounds = GetBounds(inScreenSpace);
            return x >= bounds.x && x <= bounds.x + bounds.width && y >= bounds.y && y <= bounds.y + bounds.height;
        }

        /// <summary>
        /// MonoBehavior Update handler.
        /// In inherited classes always use base.Update() when overriding this method.
        /// </summary>
        protected virtual void Update() { }

        /// <summary>
        /// MonoBehavior Awake handler.
        /// In inherited classes always use base.Awake() when overriding this method.
        /// </summary>
        protected virtual void Awake() { }

        /// <summary>
        /// MonoBehavior Start handler.
        /// In inherited classes always use base.Start() when overriding this method.
        /// </summary>
        protected virtual void Start() { }

        /// <summary>
        /// MonoBehavior OnDisable handler.
        /// In inherited classes always use base.OnDisable() when overriding this method.
        /// </summary>
        protected virtual void OnDisable() { }

        /// <summary>
        /// MonoBehavior OnValidate handler.
        /// In inherited classes always use base.OnValidate() when overriding this method.
        /// </summary>
        protected virtual void OnValidate() { }

        /// <summary>
        /// MonoBehavior Reset handler.
        /// In inherited classes always use base.Reset() when overriding this method.
        /// </summary>
        protected virtual void Reset() { }
    }
}
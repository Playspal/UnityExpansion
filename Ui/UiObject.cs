using System;
using System.Collections.Generic;

using UnityEngine;
using UnityExpansion.UI.Animation;

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
    public class UiObject : MonoBehaviour
    {
        /// <summary>
        /// Is game object destroyed.
        /// </summary>
        public bool IsDestroyed
        {
            get
            {
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
                SetPosition(value, Y);
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
                SetPosition(X, value);
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
        /// Set the parent of the Graphics transform.
        /// </summary>
        /// <param name="parent">The parent Transform to use</param>
        /// <param name="worldPositionStays">If true, the parent-relative position, scale and rotation are modified such that the object keeps the same world space position, rotation and scale as before.</param>
        public void SetParent(RectTransform parent, bool worldPositionStays = false)
        {
            RectTransform.SetParent(parent, worldPositionStays);
        }

        /// <summary>
        /// Sets the Graphics anchored position.
        /// </summary>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        public void SetPosition(float x, float y)
        {
            RectTransform.anchoredPosition = new Vector2(x, y);
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
        /// Searches a child by name and returns it.
        /// </summary>
        /// <param name="name">Name of child to be found</param>
        /// <returns>Instance of GameObject or null</returns>
        public GameObject Find(string name)
        {
            Transform output = RectTransform.Find(name);
            return output == null ? null : output.gameObject;
        }

        /// <summary>
        /// Searches for a child by name and returns specified Component attached to it.
        /// Shorter analogue of Find(name).GetComponent<type>();
        /// </summary>
        /// <param name="name">Name of child to be found</param>
        /// <returns>Instance of Component or null</returns>
        public T FindComponent<T>(string name) where T : Component
        {
            return Find(name).GetComponent<T>() as T;
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
        /// Loads and instantiates UiObject stored at path in a Resources folder.
        /// Throws exception if prefab not found at provided path.
        /// </summary>
        /// <param name="path">Prefab path in a Resources folder</param>
        /// <param name="parent">Parent RectTransform</param>
        /// <returns>Instance of UiObject</returns>
        public static T Instantiate<T>(string path, RectTransform parent) where T : UiObject
        {
            GameObject prefab = Resources.Load<GameObject>(path);

            if (prefab == null)
            {
                throw new Exception("Prefab not found at " + path);
            }

            return Instantiate<T>(prefab, parent);
        }

        /// <summary>
        /// Instantiates already loaded UiObject.
        /// Throws exception if provided type of component is not found on profided prefab.
        /// </summary>
        /// <param name="path">Prefab GameObject</param>
        /// <param name="parent">Parent RectTransform</param>
        /// <returns>Instance of UiObject</returns>
        public static T Instantiate<T>(GameObject prefab, RectTransform parent) where T : UiObject
        {
            GameObject instance = GameObject.Instantiate(prefab);

            instance.transform.SetParent(parent, false);
            instance.SetActive(true);

            T output = instance.GetComponent<T>();

            if (output == null)
            {
                throw new Exception(typeof(T).ToString() + " component doesn't found on " + prefab.name);
            }

            return output;
        }
    }
}
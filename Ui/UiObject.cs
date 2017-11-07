using UnityEngine;

namespace UnityExpansion.UI
{
    /// <summary>
    /// UnityExpansion.UI base object
    /// </summary>
    public class UiObject
    {
        /// <summary>
        /// Assosiated GameObject.
        /// </summary>
        public GameObject Graphics;

        /// <summary>
        /// RectTransform of assosiated GameObject.
        /// </summary>
        public RectTransform GraphicsTransform;

        /// <summary>
        /// Is the Graphics active.
        /// </summary>
        public bool IsActive = true;

        /// <summary>
        /// Is the Graphics visible.
        /// </summary>
        public bool IsVisible = false;

        /// <summary>
        /// Is the Graphics destroyed and UiObject ready to be removed.
        /// </summary>
        public bool IsDestroyed = false;

        /// <summary>
        /// Anchored position X.
        /// </summary>
        public float X
        {
            get
            {
                return GraphicsTransform.anchoredPosition.x;
            }

            set
            {
                SetPosition(value, Y);
            }
        }

        /// <summary>
        /// Anchored position Y.
        /// </summary>
        public float Y
        {
            get
            {
                return GraphicsTransform.anchoredPosition.y;
            }

            set
            {
                SetPosition(X, value);
            }
        }

        /// <summary>
        /// Graphics width.
        /// </summary>
        public float Width
        {
            get
            {
                return GraphicsTransform.rect.width;
            }
        }

        /// <summary>
        /// Graphics height.
        /// </summary>
        public float Height
        {
            get
            {
                return GraphicsTransform.rect.height;
            }
        }

        /// <summary>
        /// Graphics size delta X.
        /// </summary>
        public float SizeX
        {
            get
            {
                return GraphicsTransform.sizeDelta.x;
            }
        }

        /// <summary>
        /// Graphics size delta Y.
        /// </summary>
        public float SizeY
        {
            get
            {
                return GraphicsTransform.sizeDelta.y;
            }
        }

        /// <summary>
        /// Graphics alpha level. Uses CanvasGroup component to set this property.
        /// </summary>
        public float Alpha
        {
            get
            {
                return Graphics.GetOrAddComponent<CanvasGroup>().alpha;
            }

            set
            {
                Graphics.GetOrAddComponent<CanvasGroup>().alpha = value;
            }
        }

        /// <summary>
        /// Destroys Graphics and mark UiObject as destroyed.
        /// </summary>
        public void Destroy()
        {
            IsDestroyed = true;

            UnityEngine.Object.Destroy(Graphics);
        }

        /// <summary>
        /// Set Graphics instance.
        /// </summary>
        /// <param name="graphics">Graphics instance</param>
        public void SetGraphics(GameObject graphics)
        {
            Graphics = graphics;
            GraphicsTransform = Graphics.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Loads and instantiates Graphics prefab from Resources.
        /// </summary>
        /// <param name="path">Pathname of the target prefab</param>
        public void LoadGraphicsSimple(string path)
        {
            GameObject screenSrc = Resources.Load(path) as GameObject;

            Graphics = UnityEngine.Object.Instantiate(screenSrc) as GameObject;
            GraphicsTransform = Graphics.GetComponent<RectTransform>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadGraphics(string path, int layer = 0)
        {
            GameObject screenSrc = Resources.Load(path) as GameObject;

            Graphics = UnityEngine.Object.Instantiate(screenSrc) as GameObject;
            GraphicsTransform = Graphics.GetComponent<RectTransform>();
            GraphicsTransform.SetParent(Ui.GetLayer(layer));
            GraphicsTransform.localScale = new Vector3(1, 1, 1);
            GraphicsTransform.anchoredPosition = new Vector3(0, 0, 0);
            GraphicsTransform.offsetMin = Vector2.zero;
            GraphicsTransform.offsetMax = Vector2.zero;
        }

        /// <summary>
        /// Sets the Graphics rotation.
        /// </summary>
        /// <param name="value">Roration in degress</param>
        public void SetRotation(float value)
        {
            Quaternion rotation = GraphicsTransform.localRotation;
            Vector3 euler = rotation.eulerAngles;

            euler.z = value;

            rotation.eulerAngles = euler;
            GraphicsTransform.localRotation = rotation;
        }

        /// <summary>
        /// Sets the sizeDelta of Graphics.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public void SetSize(float x, float y)
        {
            GraphicsTransform.sizeDelta = new Vector2(x, y);
        }

        /// <summary>
        /// Sets the same scale of the Graphics.
        /// </summary>
        /// <param name="x">Horizontal scale</param>
        /// <param name="y">Vertical scale</param>
        public void SetScale(float x, float y)
        {
            GraphicsTransform.localScale = new Vector2(x, y);
        }

        /// <summary>
        /// Sets the same scale of the Graphics in all dimensions.
        /// </summary>
        /// <param name="value">Scale value</param>
        public void SetScale(float value)
        {
            SetScale(value, value);
        }

        /// <summary>
        /// Set the parent of the Graphics transform.
        /// </summary>
        /// <param name="parent">The parent Transform to use</param>
        /// <param name="reset">Reset Graphics current scale to one and position to zero</param>
        public void SetParent(RectTransform parent, bool reset = true)
        {
            GraphicsTransform.SetParent(parent);

            if (reset)
            {
                SetScale(1, 1);
                SetPosition(0, 0);
            }
        }

        /// <summary>
        /// Sets the Graphics anchored position.
        /// </summary>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        public void SetPosition(float x, float y)
        {
            GraphicsTransform.anchoredPosition = new Vector2(x, y);
        }

        /// <summary>
        /// Activates/Deactivates the UiObject.
        /// </summary>
        /// <param name="value">Activate or deactivation the object</param>
        public void SetActive(bool value)
        {
            if (value != IsActive)
            {
                IsActive = value;
                Graphics.SetActive(value);
            }
        }

        /// <summary>
        /// Makes Graphics invisible, but keeps all processes and animations.
        /// </summary>
        /// <param name="value">Visible or invisible</param>
        public void SetVisible(bool value)
        {
            if(value != IsVisible)
            {
                IsVisible = value;

                if (value)
                {
                    SetPosition(X * 1000, Y * 1000);
                }
                else
                {
                    SetPosition(X / 1000, Y / 1000);
                }
            }
        }

        /// <summary>
        /// Finds a child by name and returns it.
        /// </summary>
        /// <param name="name">Name of child to be found</param>
        /// <returns>Instance of GameObject or null</returns>
        public GameObject Find(string name)
        {
            Transform output = GraphicsTransform.Find(name);
            return output == null ? null : output.gameObject;
        }

        /// <summary>
        /// Finds a child by name and returns specified Component attached to it.
        /// </summary>
        /// <param name="name">Name of child to be found</param>
        /// <returns>Instance of Component or null</returns>
        public T FindComponent<T>(string name) where T : Component
        {
            return Find(name).GetComponent<T>() as T;
        }
    }
}
using UnityEngine;

public static class ExtensionsRectTransform
{
    /// <summary>
    /// Gets the bound box of object in screen space.
    /// </summary>
    public static Rect GetBoundsInScreenSpace(this RectTransform rectTransform)
    {
        Vector3[] verticesWorld = new Vector3[4];
        Vector3[] verticesScreen = new Vector3[4];

        rectTransform.GetWorldCorners(verticesWorld);
        verticesScreen = rectTransform.GetCamera().WorldToScreenPoints(verticesWorld);

        return new Rect
        (
            verticesScreen[0].x,
            verticesScreen[0].y,
            verticesScreen[2].x - verticesScreen[0].x,
            verticesScreen[2].y - verticesScreen[0].y
        );
    }

    /// <summary>
    /// Sets the position in screen space.
    /// </summary>
    /// <param name="x">X coordinate in screen space</param>
    /// <param name="y">Y coordinate in screen space</param>
    public static void SetPositionInScreenSpace(this RectTransform rectTransform, float x, float y)
    {
        Vector2 point;

        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rectTransform.parent.GetComponent<RectTransform>(),
            new Vector2(x, y),
            rectTransform.GetCamera(),
            out point
        );

        rectTransform.localPosition = new Vector3(point.x, point.y, rectTransform.localPosition.z);
    }

    /// <summary>
    /// Gets parent canvas of this RectTransform.
    /// </summary>
    public static Canvas GetCanvas(this RectTransform rectTransform)
    {
        return rectTransform.GetComponentInParent<Canvas>();
    }

    /// <summary>
    /// Gets parent camera.
    /// Canvas render mode should be "Screen Space - Camera".
    /// </summary>
    public static Camera GetCamera(this RectTransform rectTransform)
    {
        return rectTransform.GetCanvas().worldCamera;
    }
}

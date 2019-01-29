using UnityEngine;

public static class ExtensionsCamera
{
    /// <summary>
    /// Transforms position from world space into screen space for an array of points.
    /// </summary>
    /// <param name="worldPoints">Array of points in world space</param>
    /// <returns></returns>
    public static Vector3[] WorldToScreenPoints(this Camera camera, Vector3[] worldPoints)
    {
        Vector3[] output = new Vector3[worldPoints.Length];

        for(int i = 0; i < worldPoints.Length; i++)
        {
            output[i] = camera.WorldToScreenPoint(worldPoints[i]);
        }

        return output;
    }
}

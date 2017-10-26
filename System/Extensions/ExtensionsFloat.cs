using UnityEngine;

public static class ExtensionsFloat
{
    /// <summary>
    /// Convert to string with leading zeros.
    /// If length of original value same or bigger than length parameter, zeros will be not added
    /// </summary>
    /// <param name="length">Target string length</param>
    public static string ToLeadingZerosString(this float value, int length = 2)
    {
        string valueAsString = value.ToString();
        int i;

        length -= valueAsString.Length;

        for (i = 0; i < length; i++)
        {
            valueAsString = "0" + valueAsString;
        }

        return valueAsString;
    }

    /// <summary>
    /// Puts variable's value to Unity PlayerPrefs
    /// </summary>
    /// <param name="id">Unique Id of variable</param>
    public static void CacheSave(this float value, string id)
    {
        PlayerPrefs.SetFloat(id, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Get value from Unity PlayerPrefs by unique Id
    /// </summary>
    /// <param name="id">Unique Id of variable</param>
    /// <param name="valueDefault">That value will be returned if PlayerPreft not contain provided Id</param>   
    public static float CacheLoad(this float value, string id, float valueDefault = 0)
    {
        return PlayerPrefs.GetFloat(id, valueDefault);
    }
}
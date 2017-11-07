using UnityEngine;

public static class ExtensionsString
{
    /// <summary>
    /// Adds spaces with provided interval.
    /// </summary>
    public static string SplitBySpaces(this string value, int interval = 3)
    {
        string output = "";

        int i;

        for (i = 0; i < value.Length; i += interval)
        {
            output += value.Substring(i, interval);
            output += " ";
        }

        return output;
    }

    /// <summary>
    /// Puts variable's value to Unity PlayerPrefs.
    /// </summary>
    /// <param name="id">Unique Id of variable</param>
    public static void CacheSave(this string value, string id)
    {
        PlayerPrefs.SetString(id, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Get value from Unity PlayerPrefs by unique Id.
    /// </summary>
    /// <param name="id">Unique Id of variable</param>
    /// <param name="valueDefault">That value will be returned if PlayerPreft not contain provided Id</param>   
    public static string CacheLoad(this string value, string id, string valueDefault = "")
    {
        return PlayerPrefs.GetString(id, valueDefault);
    }
}

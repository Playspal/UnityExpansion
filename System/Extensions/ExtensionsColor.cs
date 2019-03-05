using UnityEngine;

public static class ExtensionsColor
{
    /// <summary>
    /// Sets color by html color string.
    /// </summary>
    /// <param name="value">Color string as #XXXXXX</param>
    public static Color Parse(this Color color, string value)
    {
        Color output = new Color();

        if(ColorUtility.TryParseHtmlString(value, out output))
        {
            return output;
        }

        return color;
    }
}

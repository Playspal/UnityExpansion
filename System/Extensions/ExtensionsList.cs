using System.Collections.Generic;

public static class ExtensionsList
{
    /// <summary>
    /// Removes first item in list and returns it value
    /// </summary>
    public static T Shift<T>(this List<T> list)
    {
        if (list.Count > 0)
        {
            T output = list[0];
            list.RemoveAt(0);

            return output;
        }

        return default(T);
    }

    /// <summary>
    /// Removes last item in list and returns it value
    /// </summary>
    public static T Pop<T>(this List<T> list)
    {
        if (list.Count > 0)
        {
            T output = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);

            return output;
        }

        return default(T);
    }
}

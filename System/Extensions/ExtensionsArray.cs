using System;

public static class ExtensionsArray
{
    /// <summary>
    /// Increases size of array by one and adds one element to the end of array.
    /// </summary>
    /// <returns>New array</returns>
    public static T[] Push<T>(this T[] array, T item)
    {
        if(array == null)
        {
            array = new T[0];
        }

        Array.Resize(ref array, array.Length + 1);

        array[array.Length - 1] = item;

        return array;
    }

    /// <summary>
    /// Removes provided elements and resizes array.
    /// </summary>
    /// <returns>New array</returns>
    public static T[] Remove<T>(this T[] array, T item)
    {
        int n = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (!array[i].Equals(item))
            {
                array[n] = array[i];
                n++;
            }
        }

        Array.Resize(ref array, n);

        return array;
    }

    /// <summary>
    /// Is the array containes provieded element.
    /// </summary>
    public static bool Exists<T>(this T[] array, T item)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if(array[i].Equals(item))
            {
                return true;
            }
        }

        return false;
    }
}

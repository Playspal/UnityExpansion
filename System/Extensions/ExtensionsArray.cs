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
}

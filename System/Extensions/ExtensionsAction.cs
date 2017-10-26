using System;

public static class ActionExtensions
{
    public static void InvokeIfNotNull<T>(this Action<T, T, T> action, T obj1, T obj2, T obj3)
    {
        if (action != null)
        {
            action(obj1, obj2, obj3);
        }
    }

    public static void InvokeIfNotNull<T>(this Action<T, T> action, T obj1, T obj2)
    {
        if (action != null)
        {
            action(obj1, obj2);
        }
    }

    public static void InvokeIfNotNull<T>(this Action<T> action, T obj)
    {
        if (action != null)
        {
            action(obj);
        }
    }

    public static void InvokeIfNotNull(this Action action)
    {
        if (action != null)
        {
            action();
        }
    }

    public static T InvokeIfNotNull<T>(this Func<T> action)
    {
        if (action != null)
        {
            return action();
        }

        return default(T);
    }
}

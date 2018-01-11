using System;

public static class ExtensionsAction
{
    public static void InvokeIfNotNull<T1, T2, T3>(this Action<T1, T2, T3> action, T1 obj1, T2 obj2, T3 obj3)
    {
        if (action != null)
        {
            action(obj1, obj2, obj3);
        }
    }

    public static void InvokeIfNotNull<T1, T2>(this Action<T1, T2> action, T1 obj1, T2 obj2)
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

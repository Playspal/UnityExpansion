using System;
using System.Reflection;

public static class ExtensionsPropertyInfo
{
    /// <summary>
    /// Sets the value if propertyInfo.CanWrite is true.
    /// </summary>
    /// <param name="target">Target object</param>
    /// <param name="value">New value</param>
    public static void SafeSetValue(this PropertyInfo propertyInfo, object target, object value)
    {
        if (propertyInfo.CanWrite)
        {
            propertyInfo.SetValue(target, Convert.ChangeType(value, propertyInfo.PropertyType), null);
        }
    }

    /// <summary>
    /// Gets the value if propertyInfo.CanRead is true.
    /// </summary>
    /// <param name="target">Target object</param>
    public static object SafeGetValue(this PropertyInfo propertyInfo, object target)
    {
        return propertyInfo.CanRead ? propertyInfo.GetValue(target, null) : null;
    }
}
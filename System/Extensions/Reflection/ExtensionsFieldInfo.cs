using System;
using System.Reflection;

public static class ExtensionsFieldInfo
{
    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="target">Target object</param>
    /// <param name="value">New value</param>
    public static void SafeSetValue(this FieldInfo fieldInfo, object target, object value)
    {
        IConvertible convertible = value as IConvertible;

        if (convertible != null)
        {
            fieldInfo.SetValue(target, Convert.ChangeType(value, fieldInfo.FieldType));
        }
        else
        {
            fieldInfo.SetValue(target, value);
        }
    }
}
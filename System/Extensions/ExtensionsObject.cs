using System;
using System.Reflection;

public static class ExtensionsObject
{
    public static void SetMemberValue(this object target, string name, object value)
    {
        Type type = target.GetType();

        BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        PropertyInfo propertyInfo = type.GetProperty(name, bindFlags);
        FieldInfo fieldInfo = type.GetField(name, bindFlags);

        if (propertyInfo != null && propertyInfo.CanWrite)
        {
            propertyInfo.SetValue(target, Convert.ChangeType(value, propertyInfo.PropertyType), null);
        }

        if (fieldInfo != null)
        {
            fieldInfo.SetValue(target, Convert.ChangeType(value, fieldInfo.FieldType));
        }
    }

    public static object GetMemberValue(this object target, string name)
    {
        Type type = target.GetType();

        BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        PropertyInfo propertyInfo = type.GetProperty(name, bindFlags);
        FieldInfo fieldInfo = type.GetField(name, bindFlags);

        if (propertyInfo != null && propertyInfo.CanRead)
        {
            return propertyInfo.GetValue(target, null);
        }

        if (fieldInfo != null)
        {
            return fieldInfo.GetValue(target);
        }

        return null;
    }
}

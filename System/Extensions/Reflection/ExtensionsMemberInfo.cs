using System.Reflection;

public static class ExtensionsMemberInfo
{
    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="target">Target object</param>
    /// <param name="value">New value</param>
    public static void SetValue(this MemberInfo memberInfo, object target, object value)
    {
        switch(memberInfo.MemberType)
        {
            case MemberTypes.Field:
                FieldInfo fieldInfo = memberInfo as FieldInfo;
                fieldInfo.SafeSetValue(target, value);
                break;

            case MemberTypes.Property:
                PropertyInfo propertyInfo = memberInfo as PropertyInfo;
                propertyInfo.SafeSetValue(target, value);
                break;
        }
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="target">Target object</param>
    public static object GetValue(this MemberInfo memberInfo, object target)
    {
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Field:
                FieldInfo fieldInfo = memberInfo as FieldInfo;
                return fieldInfo.GetValue(target);

            case MemberTypes.Property:
                PropertyInfo propertyInfo = memberInfo as PropertyInfo;
                return propertyInfo.SafeGetValue(target);
        }

        return null;
    }
}

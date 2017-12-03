using System;
using System.Reflection;

namespace UnityExpansion.Utilities
{
    /// <summary>
    /// Provides basic reflection functionality.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityEngine;
    /// using UnityExpansion.Utilities;
    /// 
    /// public class MyClass1
    /// {
    ///     public int MyField;
    /// }
    /// 
    /// public class MyClass2
    /// {
    ///     public MyClass2()
    ///     {
    ///         MyClass1 a = new MyClass1();
    ///         MyClass1 b = new MyClass1();
    ///         
    ///         // Sets a.MyField value
    ///         UtilityReflection.SetMemberValue(a, "MyField", 100);
    ///         
    ///         // Copy all properties and fields from a to b
    ///         UtilityReflection.CloneMembers(a, b);
    ///         
    ///         // Prints 100
    ///         Debug.Log(UtilityReflection.GetMemberValue(b, "MyField"));
    ///     }
    /// }
    /// </code>
    /// </example>
    public static class UtilityReflection
    {
        /// <summary>
        /// Clones all properties and fields from one object to another.
        /// </summary>
        /// <param name="from">From object</param>
        /// <param name="to">To object</param>
        public static void CloneMembers(object from, object to)
        {
            Type typeFrom = from.GetType();
            Type typeTo = to.GetType();

            if(typeFrom != typeTo)
            {
                return;
            }

            PropertyInfo[] properties = typeFrom.GetProperties();
            FieldInfo[] fields = typeFrom.GetFields();

            foreach (PropertyInfo property in properties)
            {
                SetMemberValue(to, property.Name, property.GetValue(from, null));
            }

            foreach (FieldInfo field in fields)
            {
                SetMemberValue(to, field.Name, field.GetValue(from));
            }
        }

        /// <summary>
        /// Sets value of specified object's property or field.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="name">Member name</param>
        /// <param name="value">New value</param>
        public static void SetMemberValue(object target, string name, object value)
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

        /// <summary>
        /// Gets value of specified object's property or field.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="name">Member name</param>
        /// <returns>Member's value or null if member with specified name is not found</returns>
        public static object GetMemberValue(object target, string name)
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
}
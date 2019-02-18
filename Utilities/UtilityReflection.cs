using System;
using System.Collections.Generic;
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
            // Hint: The only way to get a private field that is declared in a base class from a derived type is to go up the class hierarchy.
            // See the "type = type.BaseType;" line in the while loop to understand it.

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty;

            Type type = target.GetType();

            while (type != null)
            {
                PropertyInfo propertyInfo = type.GetProperty(name, bindingFlags);
                FieldInfo fieldInfo = type.GetField(name, bindingFlags);

                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(target, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                    break;
                }

                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(target, Convert.ChangeType(value, fieldInfo.FieldType));
                    break;
                }

                type = type.BaseType;
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

        /// <summary>
        /// Executes specified method.
        /// </summary>
        public static void ExecuteMethod(object target, string name, object[] parameters = null)
        {
            Type type = target.GetType();

            MethodInfo method = type.GetMethod(name);

            if (method != null)
            {
                method.Invoke(target, parameters);
            }
        }

        /// <summary>
        /// Gets array of methods names that have specified attribute.
        /// </summary>
        /// <returns>Array of names</returns>
        public static string[] GetMethodsWithAttribute(object target, Type attributeType)
        {
            List<string> output = new List<string>();

            Type type = target.GetType();
            MethodInfo[] methods = type.GetMethods();

            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i].GetCustomAttributes(attributeType, true).Length > 0)
                {
                    output.Add(methods[i].Name);
                }
            }

            return output.ToArray();
        }

        public static string[] GetEventsWithAttribute(object target, Type attributeType)
        {
            List<string> output = new List<string>();

            Type type = target.GetType();
            EventInfo[] events = type.GetEvents();

            for (int i = 0; i < events.Length; i++)
            {
                if (events[i].GetCustomAttributes(attributeType, true).Length > 0)
                {
                    output.Add(events[i].Name);
                }
            }

            return output.ToArray();
        }

        public static Attribute GetMethodAttribute(object target, string name, Type attributeType)
        {
            Type type = target.GetType();
            MethodInfo method = type.GetMethod(name);

            if (method != null)
            {
                object[] attributes = method.GetCustomAttributes(attributeType, true);

                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i].GetType() == attributeType)
                    {
                        return attributes[i] as Attribute;
                    }
                }
            }

            return null;
        }

        public static Attribute GetEventAttribute(object target, string name, Type attributeType)
        {
            Type type = target.GetType();
            EventInfo method = type.GetEvent(name);

            if (method != null)
            {
                object[] attributes = method.GetCustomAttributes(attributeType, true);

                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i].GetType() == attributeType)
                    {
                        return attributes[i] as Attribute;
                    }
                }
            }

            return null;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace UnityExpansion.Utilities
{
    /// <summary>
    /// Provides advanced reflection functionality.
    /// </summary>
    public static class UtilityReflection
    {
        /// <summary>
        /// Executes specified method.
        /// </summary>
        public static void ExecuteMethod(object target, string name, object[] parameters = null)
        {
            Type type = target.GetType();
            MethodInfo method = type.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (method != null)
            {
                method.Invoke(target, parameters);
            }
        }

        /// <summary>
        /// Adds handler delegate to specified event.
        /// </summary>
        public static void AddEventHandler(object target, string eventName, Action handler)
        {
            Type type = target.GetType();
            EventInfo eventInfo = type.GetEvent(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (eventInfo != null)
            {
                eventInfo.AddEventHandler(target, handler);
            }
        }

        /// <summary>
        /// Clones all properties and fields from one object to another.
        /// </summary>
        /// <param name="from">Target object</param>
        /// <param name="to">Destination object</param>
        public static void CloneMembers(object from, object to)
        {
            if (from.GetType() == to.GetType())
            {
                MemberTypes[] memberTypes = new MemberTypes[] { MemberTypes.Property, MemberTypes.Field };
                MemberInfo[] members = GetMembers(from, memberTypes);

                for (int i = 0; i < members.Length; i++)
                {
                    members[i].SetValue(to, members[i].GetValue(from));
                }
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
            MemberInfo memberInfo = GetMember(target, name);

            if (memberInfo != null)
            {
                memberInfo.SetValue(target, value);
            }
        }

        /// <summary>
        /// Gets value of specified member.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="name">Member name</param>
        /// <returns>Member's value or null if member with specified name is not found</returns>
        public static object GetMemberValue(object target, string name)
        {
            MemberInfo member = GetMember(target, name);
            return member != null ? member.GetValue(target) : null;
        }

        /// <summary>
        /// Gets value of specified member with specified index.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="name">Member name</param>
        /// <param name="name">Member index</param>
        /// <returns>Member's value or null if member with specified name is not found</returns>
        public static object GetMemberValue(object target, string name, int index)
        {
            IEnumerable enumerable = GetMemberValue(target, name) as IEnumerable;

            if (enumerable == null)
            {
                return null;
            }

            IEnumerator enumerator = enumerable.GetEnumerator();

            for (int i = 0; i <= index; i++)
            {
                if (!enumerator.MoveNext())
                {
                    return null;
                }
            }

            return enumerator.Current;
        }

        /// <summary>
        /// Gets member with specified name.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="name">Member name</param>
        public static MemberInfo GetMember(object target, string name)
        {
            MemberInfo[] members = GetMembers(target);

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] != null && members[i].Name == name)
                {
                    return members[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Get list of all members.
        /// </summary>
        /// <param name="target">Target object</param>
        public static MemberInfo[] GetMembers(object target)
        {
            return target.GetType().GetAllMembers();
        }

        /// <summary>
        /// Gets list of specified members.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="memberTypes">Array of required members types</param>
        public static MemberInfo[] GetMembers(object target, MemberTypes[] memberTypes)
        {
            return target.GetType().GetAllMembers(memberTypes);
        }

        /// <summary>
        /// Gets array of members names with specified attribute.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="attributeType">Specified attribute</param>
        public static string[] GetMembersWithAttribute(object target, Type attributeType)
        {
            List<string> output = new List<string>();
            MemberInfo[] members = GetMembers(target);

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].GetCustomAttributes(attributeType, true).Length > 0)
                {
                    if (!output.Contains(members[i].Name))
                    {
                        output.Add(members[i].Name);
                    }
                }
            }

            return output.ToArray();
        }

        /// <summary>
        /// Gets specified attribute attached to member with specified name.
        /// </summary>
        /// <typeparam name="T">Type of required attribute</typeparam>
        /// <param name="target">Target object</param>
        /// <param name="memberName">Member name</param>
        public static T GetAttribute<T>(object target, string memberName) where T : Attribute
        {
            return GetAttribute(target, memberName, typeof(T)) as T;
        }

        /// <summary>
        /// Gets specified attribute attached to member with specified name.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="memberName">Member name</param>
        /// <param name="attributeType">Type of required attribute</param>
        public static Attribute GetAttribute(object target, string memberName, Type attributeType)
        {
            MemberInfo member = GetMember(target, memberName);

            if(member != null)
            {
                object[] attributes = member.GetCustomAttributes(attributeType, true);

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
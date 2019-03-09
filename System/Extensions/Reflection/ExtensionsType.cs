using System;
using System.Collections.Generic;
using System.Reflection;

public static class ExtensionsType
{
    /// <summary>
    /// Get list of all members.
    /// </summary>
    public static MemberInfo[] GetAllMembers(this Type type)
    {
        return type.GetAllMembers(null);
    }

    /// <summary>
    /// Gets list of specified members.
    /// </summary>
    /// <param name="memberTypes">Array of required members types</param>
    public static MemberInfo[] GetAllMembers(this Type type, MemberTypes[] memberTypes)
    {
        HashSet<MemberTypes> filter = memberTypes != null ? new HashSet<MemberTypes>(memberTypes) : null;
        HashSet<MemberInfo> found = GetAllMembers(type, filter);

        MemberInfo[] output = new MemberInfo[found.Count];
        found.CopyTo(output);

        return output;
    }

    /// <summary>
    /// Gets all members.
    /// While loop required to access private members of base types.
    /// Code looks excess, but it is made for better performance.
    /// </summary>
    /// <param name="type">Target type</param>
    /// <param name="filter">HashSet of required members types, pass null if all types is required</param>
    /// <returns>HashSet of found and filtered members</returns>
    private static HashSet<MemberInfo> GetAllMembers(Type type, HashSet<MemberTypes> filter)
    {
        BindingFlags bindingFlags =
        (
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.GetField |
            BindingFlags.GetProperty
        );

        HashSet<MemberInfo> output = new HashSet<MemberInfo>();

        if (filter == null)
        {
            while (type != null)
            {
                MemberInfo[] members = type.GetMembers(bindingFlags);

                for (int i = 0; i < members.Length; i++)
                {
                    if (!output.Contains(members[i]))
                    {
                        output.Add(members[i]);
                    }
                }

                type = type.BaseType;
            }
        }
        else
        {
            while (type != null)
            {
                MemberInfo[] members = type.GetMembers(bindingFlags);

                for (int i = 0; i < members.Length; i++)
                {
                    if (filter.Contains(members[i].MemberType) && !output.Contains(members[i]))
                    {
                        output.Add(members[i]);
                    }
                }

                type = type.BaseType;
            }
        }

        return output;
    }
}

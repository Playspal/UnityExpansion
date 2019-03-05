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
        return type.GetAllMembers(Enum.GetValues(typeof(MemberTypes)) as MemberTypes[]);
    }

    /// <summary>
    /// Gets list of specified members.
    /// </summary>
    /// <param name="memberTypes">Array of required members types</param>
    public static MemberInfo[] GetAllMembers(this Type type, MemberTypes[] memberTypes)
    {
        BindingFlags bindingFlags =
        (
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.GetField |
            BindingFlags.GetProperty
        );

        List<MemberInfo> output = new List<MemberInfo>();
        List<MemberTypes> filter = new List<MemberTypes>(memberTypes);

        // Loop required to access private members of base types
        while (type != null)
        {
            MemberInfo[] members = type.GetMembers(bindingFlags);

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] != null)
                {
                    if (filter.Contains(members[i].MemberType) && !output.Contains(members[i]))
                    {
                        output.Add(members[i]);
                    }
                }
            }

            type = type.BaseType;
        }

        return output.ToArray();
    }
}

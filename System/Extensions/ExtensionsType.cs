using System;

public static class ExtensionsType
{
    public static bool IsInheritedFrom(this Type type, Type parentType)
    {
        while (type != null)
        {
            if(type == parentType)
            {
                return true;
            }

            type = type.BaseType;
        }

        return false;
    }
}

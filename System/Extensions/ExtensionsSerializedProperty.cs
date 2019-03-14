using UnityEditor;

using UnityExpansion.Utilities;

public static class ExtensionsSerializedProperty
{
    public static object GetObject(this SerializedProperty serializedProperty)
    {
        object targetObject = serializedProperty.serializedObject.targetObject;

        string path = serializedProperty.propertyPath.Replace(".Array.data[", "[");
        string[] elements = path.Split('.');

        for (int i = 0; i < elements.Length; i++)
        {
            string element = elements[i];

            if (element.Contains("["))
            {
                string elementName = element.Substring(0, element.IndexOf("["));
                int index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));

                targetObject = UtilityReflection.GetMemberValue(targetObject, elementName, index);
            }
            else
            {
                targetObject = UtilityReflection.GetMemberValue(targetObject, element);
            }
        }

        return targetObject;
    }
}
using UnityEditor;
using UnityEngine;

using UnityExpansion;
using UnityExpansion.Utilities;

namespace UnityExpansionInternal
{
    [CustomPropertyDrawer(typeof(PersistantID))]
    public class InspectorPersistantID : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PersistantID persistantID = GetSerializedPropertyObject(property) as PersistantID;

            string tooltip = "PersistantID: " + persistantID.ToString();

            position = EditorGUI.PrefixLabel
            (
                position,
                GUIUtility.GetControlID(FocusType.Passive),
                new GUIContent(label.text, tooltip)
            );

            GUI.Label(position, new GUIContent(persistantID.ToString(), tooltip));
        }

        private static object GetSerializedPropertyObject(SerializedProperty prop)
        {
            string[] path = prop.propertyPath.Split('.');

            object targetObject = prop.serializedObject.targetObject;

            for (int i = 0; i < path.Length; i++)
            {
                targetObject = UtilityReflection.GetMemberValue(targetObject, path[i]);
            }

            return targetObject;
        }
    }
}
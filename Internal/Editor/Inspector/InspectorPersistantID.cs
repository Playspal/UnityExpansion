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

            string tooltip = "PersistantID: " + persistantID.ToString();

            position = EditorGUI.PrefixLabel
            (
                position,
                GUIUtility.GetControlID(FocusType.Passive),
                new GUIContent(label.text, tooltip)
            );

            GUI.Label(position, new GUIContent(persistantID.ToString(), tooltip));
        }
    }
}
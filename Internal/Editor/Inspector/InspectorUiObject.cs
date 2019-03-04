using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;
using UnityEngine;

using UnityExpansion.UI;
using UnityExpansion.Utilities;

namespace UnityExpansionInternal
{
    [CustomEditor(typeof(UiObject), true)]
    public class InspectorUiObject : Editor
    {
        private const string MESSAGE_DRAG_AND_DROP = "\nDrag and drop here to safe replace {0} to {1}. All serialized properties will be saved.\n";

        private Type _draggedType = null;

        public override void OnInspectorGUI()
        {
            // Render message
            if (_draggedType != null)
            {
                string message = string.Format
                (
                    MESSAGE_DRAG_AND_DROP,
                    target.GetType().Name,
                    _draggedType.Name
                );

                EditorGUILayout.HelpBox(message, MessageType.Info);
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            }

            // Render default inspector
            base.OnInspectorGUI();

            // Process events
            switch (Event.current.type)
            {
                case EventType.DragUpdated:
                    OnDragUpdated();
                    break;

                case EventType.DragPerform:
                    OnDragPerform();                    
                    break;

                case EventType.DragExited:
                    OnDragExited();
                    break;
            }
        }

        private void OnDragUpdated()
        {
            _draggedType = GetDraggedObjectType();

            if (_draggedType != null)
            {
                Event.current.Use();
            }
        }

        private void OnDragPerform()
        {
            if (_draggedType != null)
            {
                ReplaceComponent(target as UiObject, _draggedType);
                Event.current.Use();
            }

            _draggedType = null;
        }

        private void OnDragExited()
        {
            _draggedType = null;
        }

        /// <summary>
        /// Returns actual Type of dragged object in case it inherited from UiObject.
        /// Returns null if object not inherited from UiObject.
        /// </summary>
        private Type GetDraggedObjectType()
        {
            if (DragAndDrop.objectReferences.Length > 0)
            {
                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                {
                    Type type = DragAndDrop.objectReferences[i].GetType();

                    if (type == typeof(MonoScript))
                    {
                        MonoScript monoScript = DragAndDrop.objectReferences[i] as MonoScript;
                        Type monoScriptType = monoScript.GetClass();

                        if (monoScriptType == typeof(UiObject) || monoScriptType.IsSubclassOf(typeof(UiObject)))
                        {
                            return monoScriptType;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Replaces provided component by new one of specified type.
        /// All fields and properties will copied to the new one.
        /// </summary>
        /// <param name="component">Component to replace</param>
        /// <param name="type">Type of new component</param>
        private void ReplaceComponent(UiObject component, Type type)
        {
            GameObject gameObject = component.gameObject;

            Dictionary<string, object> members = GetMembersValues(component);

            DestroyImmediate(component);

            component = gameObject.AddComponent(type) as UiObject;

            SetMembersValues(component, members);
        }

        /// <summary>
        /// Set object's members by Dictionary of name / value pairs.
        /// </summary>
        private void SetMembersValues(UiObject target, Dictionary<string, object> values)
        {
            foreach (KeyValuePair<string, object> item in values)
            {
                UtilityReflection.SetMemberValue(target, item.Key, item.Value);
            }
        }

        /// <summary>
        /// Get Dictionary of object's defined members.
        /// </summary>
        private Dictionary<string, object> GetMembersValues(UiObject target)
        {
            Dictionary<string, object> output = new Dictionary<string, object>();

            // Filter required to ignore members of UiObject and it's base classes
            List<string> filter = GetMembersNames(typeof(UiObject));

            MemberInfo[] members = GetMembers(target.GetType());

            for (int i = 0; i < members.Length; i++)
            {
                string name = members[i].Name;

                if (!filter.Contains(name) && !output.ContainsKey(name))
                {
                    output.Add
                    (
                        name,
                        members[i].GetValue(target)
                    );
                }
            }

            return output;
        }

        /// <summary>
        /// Gets list of type's fields and properties names.
        /// </summary>
        private List<string> GetMembersNames(Type type)
        {
            List<string> output = new List<string>();

            MemberInfo[] members = GetMembers(type);

            for(int i = 0; i < members.Length; i++)
            {
                string name = members[i].Name;

                if (!output.Contains(name))
                {
                    output.Add(name);
                }
            }

            return output;
        }

        /// <summary>
        /// Gets type's fields and properties.
        /// </summary>
        private MemberInfo[] GetMembers(Type type)
        {
            return type.GetAllMembers
            (
                new MemberTypes[]
                {
                    MemberTypes.Field,
                    MemberTypes.Property
                }
            );
        }
    }
}
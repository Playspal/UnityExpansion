using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace UnityExpansion
{
    /// <summary>
    /// This class used to recursively search all PersistantID attached to GameObject.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityEngine;
    /// using UnityExpansion;
    /// 
    /// public static class MyClass
    /// {
    ///     public static void ParseObject(GameObject gameObject)
    ///     {
    ///         PersistantIDExplorer.Explore
    ///         (
    ///             gameObject,
    ///             (object foundObject, PersistantID persistantID) =>
    ///             {
    ///                 Debug.Log("Found " + foundObject + " with ID: " + persistantID.ToString());
    ///             }
    ///         );
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="UnityExpansion.PersistantID" />
    public class PersistantIDExplorer
    {
        private const BindingFlags BINDING_FLAGS =
        (
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.GetField |
            BindingFlags.GetProperty
        );

        /// <summary>
        /// Explores the specified game object.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <param name="onFound">The on found.</param>
        public static void Explore(GameObject gameObject, Action<object, PersistantID> onFound)
        {
            ExploreGameObjects(gameObject, onFound, new Stack<object>());
        }

        private static void ExploreGameObjects(GameObject gameObject, Action<object, PersistantID> onFound, Stack<object> filter)
        {
            Transform transform = gameObject.transform;

            // Gets list of attached components
            Component[] components = gameObject.GetComponents(typeof(Component));

            // Parse members of components game object
            for (int i = 0; i < components.Length; i++)
            {
                ExploreMembers(gameObject, components[i], onFound, filter);
            }

            // Parse childs of target game object
            for (int i = 0; i < transform.childCount; i++)
            {
                ExploreGameObjects(transform.GetChild(i).gameObject, onFound, filter);
            }
        }

        private static void ExploreMembers(object parent, object target, Action<object, PersistantID> onFound, Stack<object> filter)
        {
            if (target == null)
            {
                return;
            }

            Type type = target.GetType();

            if (filter.Contains(target))
            {
                return;
            }

            filter.Push(target);

            while (type != null)
            {
                if (!IsTypeValid(type))
                {
                    return;
                }

                if (type == typeof(PersistantID))
                {
                    onFound.InvokeIfNotNull(parent, target as PersistantID);
                }

                // TODO: look inside of any collecion, not only arrays
                if (type == typeof(Array))
                {
                    Array array = target as Array;

                    for(int i = 0; i < array.Length; i++)
                    {
                        ExploreMembers(target, array.GetValue(i), onFound, filter);
                    }
                }

                FieldInfo[] fields = type.GetFields(BINDING_FLAGS);

                for (int i = 0; i < fields.Length; i++)
                {
                    ExploreMembers(target, fields[i].GetValue(target), onFound, filter);
                }

                type = type.BaseType;
            }
        }

        private static bool IsTypeValid(Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (type.IsPrimitive)
            {
                return false;
            }

            // TODO: generate list of ignored namespaces in constructor
            if (type.Namespace != null && type.Namespace == "UnityEngine")
            {
                return false;
            }

            return true;
        }
    }
}
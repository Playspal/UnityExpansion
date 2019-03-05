using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using UnityEditor;
using UnityEngine;

using UnityExpansion;

namespace UnityExpansionInternal
{
    /// <summary>
    /// This class start background service that track duplication
    /// of MonoBehaviours and reseting values of all instances of PersistantID.
    /// This is required to avoid having several instances of PersistantID with same ID.
    /// May seriously affect on performance of object duplication in case object contains
    /// huge amount of childs and components. But this is the price of Persistant ID.
    /// </summary>
    [InitializeOnLoad]
    public class ServicePersistantID : AssetPostprocessor
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty;

        // TODO: use unity native constancts instead this
        private const string COMMAND_NAME_DUPLICATE = "Duplicate";
        private const string COMMAND_NAME_PASTE = "Paste";

        // Name of the assets floder
        private const string FOLDER_NAME_ASSETS = "Assets";

        // Data path without Assets folder
        private static string _dataPath;

        // Duplication trigger
        private static bool _duplicated = false;

        // Already seen objects to filter in recursive search
        private static Stack<object> _seen = new Stack<object>();

        static ServicePersistantID()
        {
            _dataPath = Application.dataPath.Replace(FOLDER_NAME_ASSETS, string.Empty);

            EditorApplication.update += Update;
            EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
        }

        private static void OnGUI(int instanceID, Rect selectionRect)
        {
            Event currentEvent = Event.current;
            
            if (currentEvent != null && currentEvent.type == EventType.ValidateCommand)
            {
                if (currentEvent.commandName == COMMAND_NAME_DUPLICATE || currentEvent.commandName == COMMAND_NAME_PASTE)
                {
                    _duplicated = true;
                }
            }
        }

        private static void Update()
        {
            if (_duplicated)
            {
                if(Selection.gameObjects != null && Selection.gameObjects.Length > 0)
                {
                    for(int i = 0; i < Selection.gameObjects.Length; i++)
                    {
                        ParseRecursivelyGameObject(Selection.gameObjects[i]);
                    }
                }

                _duplicated = false;
                _seen.Clear();
            }
        }

        private static void ParseRecursivelyGameObject(GameObject target)
        {
            Component[] components = target.GetComponents(typeof(Component));

            // Parse members
            for (int i = 0; i < components.Length; i++)
            {
                ParseRecursively(components[i]);
            }

            // Parse childs
            for(int i = 0; i < target.transform.childCount; i++)
            {
                ParseRecursivelyGameObject(target.transform.GetChild(i).gameObject);
            }
        }

        private static void ParseRecursively(object target)
        {
            if(target == null)
            {
                return;
            }

            Type type = target.GetType();

            if(_seen.Contains(target))
            {
                return;
            }

            _seen.Push(target);

            while (type != null)
            {
                if (!IsTypeValid(type))
                {
                    return;
                }

                if (type == typeof(PersistantID))
                {
                    type.GetMethod("Generate").Invoke(target, null);
                }

                FieldInfo[] fields = type.GetFields(BINDING_FLAGS);

                for (int i = 0; i < fields.Length; i++)
                {
                    ParseRecursively(fields[i].GetValue(target));
                }

                type = type.BaseType;
            }
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            DateTime timeNow = DateTime.Now;

            foreach (string str in importedAssets)
            {
                if (str.Contains(".prefab"))
                {
                    string path = _dataPath + str + ".meta";

                    DateTime timeCreated = File.GetLastAccessTime(path);
                    TimeSpan timeDelta = timeNow - timeCreated;

                    if(timeDelta.TotalSeconds < 2)
                    {
                        GameObject gameObject = AssetDatabase.LoadAssetAtPath(str, typeof(GameObject)) as GameObject;

                        if (gameObject != null)
                        {
                            ParseRecursivelyGameObject(gameObject);
                        }
                    }
                }
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
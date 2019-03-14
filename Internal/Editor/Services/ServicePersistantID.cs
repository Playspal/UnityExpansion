using System;
using System.IO;

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
        // TODO: use unity native constancts instead this
        private const string COMMAND_NAME_DUPLICATE = "Duplicate";
        private const string COMMAND_NAME_PASTE = "Paste";

        // Name of the assets floder
        private const string FOLDER_NAME_ASSETS = "Assets";

        // Data path without Assets folder
        private static string _dataPath;

        // Duplication trigger
        private static bool _duplicated = false;

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
                        OnDublicateFound(Selection.gameObjects[i]);
                    }
                }

                _duplicated = false;
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

                    DateTime timeCreated = File.GetCreationTime(path);
                    TimeSpan timeDelta = timeNow - timeCreated;

                    if(timeDelta.TotalSeconds < 2)
                    {
                        GameObject gameObject = AssetDatabase.LoadAssetAtPath(str, typeof(GameObject)) as GameObject;

                        if (gameObject != null)
                        {
                            OnDublicateFound(gameObject);
                        }
                    }
                }
            }
        }

        private static void OnDublicateFound(GameObject gameObject)
        {
            PersistantIDExplorer.Explore
            (
                gameObject,
                OnPersistantIDFound
            );
        }

        private static void OnPersistantIDFound(object targetObject, PersistantID persistantID)
        {
            persistantID.Generate();
        }
    }
}
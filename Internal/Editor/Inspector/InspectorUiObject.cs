using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI;

namespace UnityExpansionInternal
{
    [CustomEditor(typeof(UiObject), true)]
    public class InspectorUiObject : Editor
    {
        private Type _draggedType = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Debug.LogError("!!!");
            if (_draggedType != null)
            {
                string message = string.Format
                (
                    "\nDrag and drop here to safe replace {0} to {1}. All serialized properties will be saved.\n",
                    target.GetType().Name,
                    _draggedType.Name
                );

                EditorGUILayout.HelpBox(message, MessageType.Info);
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            }

            switch (Event.current.type)
            {
                case EventType.DragUpdated:
                    OnDragUpdated();

                    if (_draggedType != null)
                    {
                        Event.current.Use();
                    }

                    break;

                case EventType.DragExited:
                    OnDragExit();
                    Event.current.Use();
                    break;
            }
        }

        private void OnDragUpdated()
        {
            _draggedType = GetDraggedUiObjectClass();
        }

        private void OnDragExit()
        {

            if (_draggedType != null)
            {
                UiObject uiObject = target as UiObject;
                GameObject go = uiObject.gameObject;

                DestroyImmediate(uiObject);
                var a = go.AddComponent(_draggedType);

                Debug.LogError("!!!" + a);
            }
            

            _draggedType = null;
        }

        private Type GetDraggedUiObjectClass()
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
                        //if(monoScriptType.IsInheritedFrom(typeof(UiObject)))
                        {
                            return monoScriptType;
                        }
                    }
                }
            }

            return null;
        }
    }
}
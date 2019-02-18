using UnityEditor;
using UnityEngine;
[InitializeOnLoad]
public class UiLayoutEditorService
{
    static UiLayoutEditorService()
    {
        EditorApplication.update += Update;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
        EditorApplication.hierarchyWindowItemOnGUI += AAA;
    }

    static void AAA(int instanceID, Rect selectionRect)
    {
        Event e = Event.current;

        if (e != null)
        {
            if ((e.type == EventType.ValidateCommand) && ((e.commandName == "Duplicate") || (e.commandName == "Paste")))
            {
                e.Use();
                Debug.LogError("!!!!! > " + Selection.activeObject);
            }
        }
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        return;
        Event e = Event.current;

        if (e != null)
        {
            Debug.LogError("!!! " + e.commandName);
            if ((e.type == EventType.ValidateCommand)
                && ((e.commandName == "Duplicate") || (e.commandName == "Paste")))
            {
                Debug.LogError("!!!!!");
            }
        }
    }

    static void Update()
    {

    }
}
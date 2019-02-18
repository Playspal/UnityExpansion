using UnityEditor;
using UnityEngine;

using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    [InitializeOnLoad]
    public class UiLayoutEditorService
    {
        // TODO: use unity native constancts instead this
        private const string COMMAND_NAME_DUPLICATE = "Duplicate";
        private const string COMMAND_NAME_PASTE = "Paste";

        // Duplication trigger
        private static bool _duplicated = false;

        static UiLayoutEditorService()
        {
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
                _duplicated = false;

                if (Selection.activeObject != null)
                {
                    UiLayoutObject layoutObject = Selection.activeGameObject.GetComponent<UiLayoutObject>();

                    if (layoutObject != null)
                    {
                        UiLayoutEditorUtils.LayoutObjectGenerateUniqueID(layoutObject);
                    }
                }
            }
        }
    }
}
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditorSelection
    {
        public UiLayout Target { get; private set; }
        public InternalUiLayoutData Data { get; private set; }

        private Object _selection = null;

        public UiLayoutEditorSelection()
        {
            Update();
        }

        public void Update()
        {
            if(Selection.activeGameObject != _selection)
            {
                if(Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<UiLayout>() == null)
                {
                    Target = null;
                    Data = null;
                }
                else
                {
                    Target = Selection.activeGameObject.GetComponent<UiLayout>();
                    Data = Target.gameObject.GetOrAddComponent<InternalUiLayoutData>();
                    Data.hideFlags = HideFlags.HideInInspector;
                }

                _selection = Selection.activeGameObject;
            }
        }
    }
}
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityExpansion.UI;

namespace UnityExpansionInternal
{
    public static class InternalUtilities
    {
        public static void SetDirty(UnityEngine.Object o)
        {
            EditorUtility.SetDirty(o);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        public static void SetDirty(GameObject go)
        {
            EditorUtility.SetDirty(go);
            EditorSceneManager.MarkSceneDirty(go.scene);
        }

        public static void SetDirty(Component comp)
        {
            EditorUtility.SetDirty(comp);
            EditorSceneManager.MarkSceneDirty(comp.gameObject.scene);
        }
    }
}
#endif
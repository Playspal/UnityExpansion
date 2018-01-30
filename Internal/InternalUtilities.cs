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

        public static List<UiLayoutSettings.Signal> GetSignals()
        {
            UiLayoutSettings[] components = GameObject.FindObjectsOfType(typeof(UiLayoutSettings)) as UiLayoutSettings[];

            if (components != null && components.Length > 0)
            {
                UiLayoutSettings layoutSettings = components[0];
                return layoutSettings.Signals;
            }

            return new List<UiLayoutSettings.Signal>();
        }

        public static string GetSignalName(string id)
        {
            List<UiLayoutSettings.Signal> signals = GetSignals();
            UiLayoutSettings.Signal signal = signals.Find(x => x.Id == id);

            if (signal != null && !string.IsNullOrEmpty(signal.Name))
            {
                return signal.Name;
            }

            return "Undefined";
        }

        public static UiLayoutSettings.Signal AddSignal(string name = "")
        {
            List<UiLayoutSettings.Signal> signals = GetSignals();

            int newid = 0;

            while (newid == 0)
            {
                newid = Random.Range(100000, 999999);

                if (signals.Find(x => x.Id == "signal" + newid) != null)
                {
                    newid = 0;
                }
            }

            UiLayoutSettings.Signal signal = new UiLayoutSettings.Signal
            {
                Id = "signal" + newid,
                Name = string.IsNullOrEmpty(name) ? "Signal #" + signals.Count : name
            };

            signals.Add(signal);

            return signal;
        }
    }
}
#endif
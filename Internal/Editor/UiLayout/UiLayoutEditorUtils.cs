using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public static class UiLayoutEditorUtils
    {
        public static string[] SignalsAdd(string[] signals, string signal)
        {
            if(string.IsNullOrEmpty(signal))
            {
                return signals;
            }

            List<string> output = new List<string>();

            bool signalAlreadyInArray = false;

            int n = 0;

            for (int i = 0; i < signals.Length; i++)
            {
                if (!string.IsNullOrEmpty(signals[i]))
                {
                    output.Add(signals[i]);
                }

                if (signals[i] == signal)
                {
                    signalAlreadyInArray = true;
                    break;
                }
            }

            if (!signalAlreadyInArray)
            {
                output.Add(signal);
            }

            return output.ToArray();
        }

        public static string[] SignalsRemove(string[] signals, string signal)
        {
            if (string.IsNullOrEmpty(signal))
            {
                return signals;
            }

            List<string> output = new List<string>();

            for(int i = 0; i < signals.Length; i++)
            {
                if(signals[i] != signal && !string.IsNullOrEmpty(signals[i]))
                {
                    output.Add(signals[i]);
                }
            }

            return output.ToArray();
        }

        public static void LayoutObjectGenerateUniqueID(UiLayoutObject layoutObject)
        {
            LayoutObjectSetUniqueID
            (
                layoutObject,
                "lo" + Random.Range(1000000, 9999999)//(layoutObject.GetInstanceID() < 0 ? "n" : "p") + Mathf.Abs(layoutObject.GetInstanceID())
            );
        }

        public static void LayoutObjectSetUniqueID(UiLayoutObject layoutObject, string uniqueID)
        {
            UnityExpansion.Utilities.UtilityReflection.SetMemberValue
            (
                layoutObject,
                "_uniqueID",
                uniqueID
            );
        }
    }
}
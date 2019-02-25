using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.UI;
using UnityExpansion.Utilities;

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
    }
}
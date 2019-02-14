﻿using System.Collections.Generic;

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

            string a = "";
            string b = "";

            //string[] output = new string[signals.Length + 1];
            List<string> output = new List<string>();

            bool signalAlreadyInArray = false;

            int n = 0;

            for (int i = 0; i < signals.Length; i++)
            {
                if (!string.IsNullOrEmpty(signals[i]))
                {
                    output.Add(signals[i]);
                }
                a += output[i] + ", ";

                if (signals[i] == signal)
                {
                    signalAlreadyInArray = true;
                    break;
                }
            }

            a += signal;

            if (!signalAlreadyInArray)
            {
                output.Add(signal);
            }

            UnityEngine.Debug.LogError(a);

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
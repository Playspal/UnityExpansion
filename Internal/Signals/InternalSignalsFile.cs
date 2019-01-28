#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using UnityEditor;
using UnityEngine;
using UnityExpansion;

namespace UnityExpansionInternal
{
    public static class InternalSignalsFile
    {
        public static void SetFile(string path)
        {
            InternalSettings[] internalSettings = GameObject.FindObjectsOfType(typeof(InternalSettings)) as InternalSettings[];

            if (internalSettings.Length > 0)
            {
                internalSettings[0].SignalsFile = path;
            }
        }

        public static string GetFile()
        {
            InternalSettings[] internalSettings = GameObject.FindObjectsOfType(typeof(InternalSettings)) as InternalSettings[];

            if(internalSettings.Length > 0 && !string.IsNullOrEmpty(internalSettings[0].SignalsFile))
            {
                return internalSettings[0].SignalsFile;
            }

            return "Assets/ExpansionSignals.cs";
        }

        public static string GetFilePath()
        {
            string path = GetFile();

            if(path.Substring(0, 7) == "Assets/")
            {
                path = path.Substring(7);
            }

            return Application.dataPath + "/" + path;
        }

        public static string GetDirrectory()
        {
            string path = GetFilePath();
            return Path.GetDirectoryName(path);
        }

        public static string GetFilename()
        {
            string path = GetFilePath();
            return Path.GetFileNameWithoutExtension(path);
        }

        public static string GetSignalName(string signalId)
        {
            List<CommonPair<string, string>> signals = Load();
            CommonPair<string, string> signal = signals.Find(x => x.Value == signalId);

            return signal != null ? signal.Key : "Undefined";
        }

        public static List<CommonPair<string, string>> Add()
        {
            List<CommonPair<string, string>> signals = Load();

            string name = "Signal" + signals.Count;
            string id = "signal" + signals.Count;

            while (true)
            {
                int n = Random.Range(100000, 999999);

                id = "signal" + n;
                name = "Signal" + n;

                CommonPair<string, string> pair = signals.Find(x => x.Value == id);

                if (pair == null)
                {
                    break;
                }
            }

            signals.Add(new CommonPair<string, string>(name, id));

            Save(signals);

            return signals;
        }

        public static void Move(string path)
        {
            List<CommonPair<string, string>> signals = Load();
            InternalIO.FileRemove(GetFilePath());
            SetFile(path);
            Save(signals);
            AssetDatabase.Refresh();
        }

        public static void Save(List<CommonPair<string, string>> signals)
        {
            string data = "";

            data += "using UnityExpansion.Services;\n\n";
            data += "public static class " + GetFilename() + "\n";
            data += "{\n";

            for (int i = 0; i < signals.Count; i++)
            {
                data += "\tpublic static Signal " + signals[i].Key + " = new Signal(\"" + signals[i].Value + "\");\n";
            }

            data += "}";

            InternalIO.FileWrite(GetFilePath(), data);
        }

        public static List<CommonPair<string, string>> Load()
        {
            string signalsFile = InternalIO.FileRead(GetFilePath());

            if (string.IsNullOrEmpty(signalsFile))
            {
                return new List<CommonPair<string, string>>();
            }
            else
            {
                return Parse(signalsFile);
            }
        }

        private static List<CommonPair<string, string>> Parse(string data)
        {
            List<CommonPair<string, string>> output = new List<CommonPair<string, string>>();

            string patternLine = "(public static Signal [A-Za-z0-9]+ = new Signal\\([A-Za-z0-9\\-\"]+\\))";
            string patternValues = "Signal (?<name>[A-Za-z0-9]+) = new Signal\\(\"(?<id>[A-Za-z0-9]+)";

            Match match = Regex.Match(data, patternLine);

            while (match.Success)
            {
                Match matchValues = Regex.Match(match.Groups[1].Value, patternValues);

                string signalName = matchValues.Groups["name"].Value;
                string signalId = matchValues.Groups["id"].Value;

                output.Add(new CommonPair<string, string>(signalName, signalId));

                match = match.NextMatch();
            }

            return output;
        }
    }
}
#endif
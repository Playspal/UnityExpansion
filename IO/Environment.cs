using UnityEngine;

namespace UnityExpansion.IO
{
    public static class Environment
    {
        /// <summary>
        /// Contains the path to a persistent accessible directory.
        /// Use it instead of UnityEngine.Application.persistentDataPath and UnityEngine.Application.dataPath.
        /// </summary>
        public static string DataPath
        {
            get
            {
                switch(Application.platform)
                {
                    case RuntimePlatform.Android:
                    case RuntimePlatform.IPhonePlayer:
                        return Application.persistentDataPath + "/";

                    default:
                        return Application.dataPath + "/";
                }
            }
        }
    }
}
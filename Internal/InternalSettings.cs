#if UNITY_EDITOR
using System;

using UnityEngine;

namespace UnityExpansionInternal
{
    [Serializable]
    public class InternalSettings : MonoBehaviour
    {
        [SerializeField]
        public string SignalsFile;

        private void Reset()
        {
            SignalsFile = "Assets/ExpansionSignals.cs";
        }
    }
}
#endif
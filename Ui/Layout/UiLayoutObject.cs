using System;
using UnityEngine;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Base class for all layout objects.
    /// Basically used to hierarchy and store persistant ID of any layout object.
    /// </summary>
    [ExecuteInEditMode]
    [Serializable]
    public class UiLayoutObject : UiObject
    {
        /// <summary>
        /// Unique ID of the object.
        /// </summary>
        public string UniqueID
        {
            get { return _uniqueID; }
        }

        /// <summary>
        /// Unique ID of this object.
        /// Generated automatically by UiLayoutEditor for system needs.
        /// </summary>
        [SerializeField, HideInInspector]
        private string _uniqueID;
    }
}
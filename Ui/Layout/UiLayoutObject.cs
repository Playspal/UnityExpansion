using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Base class for all layout objects.
    /// </summary>
    [ExecuteInEditMode]
    [System.Serializable]
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
        //[SerializeField, HideInInspector]
        [SerializeField]
        private string _uniqueID;

        [SerializeField]
        public List<UiLayoutObject> RegisteredChilds = new List<UiLayoutObject>();
    }
}
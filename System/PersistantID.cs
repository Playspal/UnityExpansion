using System;

using UnityEngine;

namespace UnityExpansion
{
    /// <summary>
    /// Class that provides persistant id for any objects.
    /// If this class will be part of component attached to prefab - all instances will be with the same ID.
    /// Use [SerializeField] attribute when use it.
    /// </summary>
    [Serializable]
    public class PersistantID
    {
        /// <summary>
        /// Value of this persistant id.
        /// </summary>
        public string Value { get { return _value; } }

        // This field will be serialized.
        [SerializeField]
        private string _value;

        /// <summary>
        /// Creates instance and generates new value.
        /// </summary>
        public PersistantID()
        {
            if (string.IsNullOrEmpty(_value))
            {
                Generate();
            }
        }

        /// <summary>
        /// Generates new uniqye value.
        /// </summary>
        public void Generate()
        {
            _value = Guid.NewGuid().ToString("N");
        }

        public override string ToString()
        {
            return _value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            if(obj is string)
            {
                return Value == (string)obj;
            }

            if(obj.GetType() == GetType())
            {
                return Value == ((PersistantID)obj).Value;
            }

            return base.Equals(obj);
        }

        public static bool operator ==(PersistantID a, PersistantID b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(PersistantID a, PersistantID b)
        {
            return a.Value != b.Value;
        }

        public static bool operator ==(PersistantID a, string b)
        {
            return a.Value == b;
        }

        public static bool operator !=(PersistantID a, string b)
        {
            return a.Value != b;
        }

        public static bool operator ==(string a, PersistantID b)
        {
            return a == b.Value;
        }

        public static bool operator !=(string a, PersistantID b)
        {
            return a != b.Value;
        }
    }
}
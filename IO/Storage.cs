using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using UnityExpansion.Utilities;

namespace UnityExpansion.IO
{
    /// <summary>
    /// Abstract class that provides storage functionality.
    /// All public dynamic members of Storage instance can be saved and loaded from local file.
    /// Private and static members will be not serialized and saved.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityExpansion.IO;
    /// 
    /// public class MyStorage : Storage
    /// {
    ///     // Default value used in case MyStorage created first time at all.
    ///     public int MyCounter = 100;
    /// }
    /// 
    /// public class MyClass
    /// {
    ///     public MyClass()
    ///     {
    ///         // Creates instance of storage and automatically loads it from file
    ///         MyStorage myStorage = new MyStorage();
    ///         
    ///         myStorage.MyCounter ++;
    ///         myStorage.Save();
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class Storage
    {
        private static List<string> _loadedInstances = new List<string>();

        /// <summary>
        /// Initializes a new instance of storage and automatically loads it's data.
        /// </summary>
        public Storage()
        {
            string type = GetType().FullName;

            // Constructor validation.
            // It required to make sure that XmlSerializer will work properly with current instance.
            ConstructorInfo[] constructors = GetType().GetConstructors();

            for (int i = 0; i < constructors.Length; i++)
            {
                if (constructors[i].GetParameters().Length > 0)
                {
                    throw (new Exception(type + " should not contain constructor that takes any arguments."));
                }
            }

            // In Load method new instance will be created and new
            // constructor will be called again because of XmlSerializer flow
            // So this code prevent infinity loop of Loads
            if (!_loadedInstances.Contains(type))
            {
                _loadedInstances.Add(type);
                Load();
                _loadedInstances.Remove(type);
            }
        }

        /// <summary>
        /// Full path of storage file
        /// </summary>
        public string FileName
        {
            get
            {
                string type = GetType().Name.ToLower();
                return Environment.DataPath + "storage." + type + ".dat";
            }
        }

        /// <summary>
        /// Serializes and saves all public members of this instance in to file.
        /// </summary>
        public void Save()
        {
            File.WriteAllText(FileName, UtilitySerialization.ObjectToXML(this));
        }

        /// <summary>
        /// Loads and assign all public members of this instance from previously saved file.
        /// If the file is empty or not exists all members will keep default values.
        /// </summary>
        public void Load()
        {
            if (File.Exists(FileName))
            {
                string xml = File.ReadAllText(FileName);
                object deserialized = UtilitySerialization.XMLToObject(xml, GetType());

                UtilityReflection.CloneMembers(deserialized, this);
            }
        }
    }
}
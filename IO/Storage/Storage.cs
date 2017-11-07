using System.Collections.Generic;
using System.IO;

namespace UnityExpansion.IO
{
    /// <summary>
    /// Abstract class that provides storage functionality.
    /// All public members of Storage instance can be saved and loaded from local file.
    /// Private members will be not serialized and saved.
    /// </summary>
    /// <example>
    /// <code>
    /// using UnityExpansion.IO;
    /// 
    /// public class MyStorage : Storage
    /// {
    ///     public int MyCounter;
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
            // In Load method new instance will be created and new
            // constructor will be called again because of XmlSerializer flow
            // So this code prevent infinity loop of Loads
            string type = GetType().FullName;

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
            File.WriteAllText(FileName, Serialization.ObjectToXML(this));
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
                var output = Serialization.XMLToObject(xml, GetType());

                foreach (var prop in output.GetType().GetProperties())
                {
                    this.SetMemberValue(prop.Name, prop.GetValue(output, null));
                }

                foreach (var prop in output.GetType().GetFields())
                {
                    this.SetMemberValue(prop.Name, prop.GetValue(output));
                }
            }
        }
    }
}
using System;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnityExpansion.Utilities
{
    /// <summary>
    /// Provides common Serialization and Deserialization functionality.
    /// Can be used to serialize any object into xml string and back.
    /// </summary>
    public static class UtilitySerialization
    {
        /// <summary>
        /// Serializes object to string using BinaryFormatter.
        /// </summary>
        /// <param name="input">Object to serialize</param>
        /// <returns>Serialized string</returns>
        public static string Serialize(object input)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, input);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        /// <summary>
        /// Deserializes object from string using BinaryFormatter.
        /// </summary>
        /// <param name="input">Serialized string</param>
        /// <returns>Deserialized object</returns>
        public static object Deserialize(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }

        /// <summary>
        /// Serializes object in to string using XmlSerializer.
        /// </summary>
        /// <param name="input">Object to serialize</param>
        /// <returns>XML string</returns>
        public static string ObjectToXML<T>(T input)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(input.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, input);
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Deserializes object from string string using XmlSerializer.
        /// </summary>
        /// <param name="xml">XML string</param>
        /// <param name="type">Object type</param>
        /// <returns>Deserialized object</returns>
        public static object XMLToObject(string xml, Type type)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);

            using (TextReader textReader = new StringReader(xml))
            {
                return xmlSerializer.Deserialize(textReader);
            }
        }
    }
}

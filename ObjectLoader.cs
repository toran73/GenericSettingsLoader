using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GenericSettingsLoader
{
    /// <summary>
    /// This class is used to quickly serialize and deserialize any object from and to XML..
    /// This is a very quick solution for any serializable object!
    /// MyType mObject= ObjectLoader<Type>.LoadFile(filename);
    /// </summary>
    /// <typeparam name="T">Object Type</typeparam>
    public static class ObjectLoader<T>
    {
        /// <summary>
        /// Loads the XML configuration.
        /// </summary>
        /// <param name="xmlText">The XML text.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>T</returns>
        /// <exception cref="Exception"></exception>
        public static T LoadXml(string xmlText, string sectionName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            try
            {
                var stream = new StringReader(xmlText);

                XmlDocument doc = new XmlDocument();
                doc.Load(stream);

                XmlNodeList node = doc.SelectNodes(sectionName);

                if (node != null && node.Count > 0 && !string.IsNullOrEmpty(node[0].InnerXml))
                {
                    // Maybe add <!xml!> tagg....
                    stream = new StringReader(node[0].InnerXml);
                    XmlTextReader xTr = new XmlTextReader(stream);

                    return (T)ser.Deserialize(xTr);
                }
                stream.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(
                    string.Format("Not possible to load XmlDocument in file {0} due to {1}", xmlText, ex.Message), ex);
            }
            return default(T);
        }

        /// <summary>
        /// Loads the XML configuration.
        /// </summary>
        /// <param name="xmlText">The XML text.</param>
        /// <returns>T</returns>
        /// <exception cref="Exception"></exception>
        public static T LoadXml(string xmlText)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            try
            {
                using (var stream = new StringReader(xmlText))
                    return (T)ser.Deserialize(stream);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(
                    string.Format("Not possible to load XmlDocument in file {0} due to {1}", xmlText, ex.Message), ex);
            }
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>T</returns>
        /// <exception cref="Exception"></exception>
        public static T LoadFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            XmlReader xr = null;
            try
            {
                xr = XmlReader.Create(filename);
                return (T)ser.Deserialize(xr);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(
                    string.Format("Not possible to load XmlDocument in file {0} due to {1}", filename, ex.Message), ex);
            }
            finally
            {
                if (xr != null) xr.Close();
            }
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="obj">The object.</param>
        /// <exception cref="Exception"></exception>
        public static void SaveFile(string filename, T obj)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            XmlWriter xw = null;
            try
            {
                xw = XmlWriter.Create(filename);
                ser.Serialize(xw, obj);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(
                    string.Format("Not possible to save XmlDocument in file {0} due to {1}", filename, ex.Message), ex);
            }
            finally
            {
                if (xw != null) xw.Close();
            }
        }
    }
}

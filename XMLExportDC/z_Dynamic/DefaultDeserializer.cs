using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace XMLExportDC
{

    /// <summary>
    /// Deserializes the given XML file into a tree of DeserializedElement objects.
    /// </summary>
    public class DefaultDeserializer
    {
        /// <summary>
        /// Given document loaded into XDocument type object.
        /// </summary>
        public XDocument Document { get; private set; }
        /// <summary>
        /// Root element of the tree.
        /// </summary>
        public DeserializedElement Root { get; private set; }

        /// <summary>
        /// Creates instance of DefaultDeserializer.
        /// </summary>
        /// <param name="document">XDocument of the XML</param>
        public DefaultDeserializer(XDocument document)
        {
            Document = document;
        }

        /// <summary>
        /// Creates instance of DefaultDeserializer.
        /// </summary>
        /// <param name="path">Input path of .xml file</param>
        public DefaultDeserializer(string path)
        {
            Document = XDocument.Load(path);
        }

        /// <summary>
        /// Deserializes the given .xml file into tree of DeserializedElement objects.
        /// </summary>
        public void Deserialize()
        {
            XElement root = Document.Root;

            Root = new DeserializedElement(root);

        }

        /// <summary>
        /// Returns a collection of every DeserializedElement with the given name.
        /// </summary>
        /// <param name="elementName">Name of the target element</param>
        /// <returns>Collection of DeserializedElement with given elementName</returns>
        public IEnumerable<DeserializedElement> Find(string elementName)
        {
            List<DeserializedElement> elementList = new List<DeserializedElement>();

            TraverseTreeAndAddElementsByName(Root, elementList, elementName);

            return elementList;
        }

        private void TraverseTreeAndAddElementsByName(DeserializedElement element, List<DeserializedElement> list, string elementName)
        {
            if (element.Name == elementName) list.Add(element);

            foreach (var child in element.Children)
            {
                TraverseTreeAndAddElementsByName(child, list, elementName);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace XMLExportDC
{
    /// <summary>
    /// Serializes DeserializedElement tree back into a XML file.
    /// </summary>
    public class DefaultSerializer
    {
        /// <summary>
        /// Root element of the tree.
        /// </summary>
        public DeserializedElement Root { get; private set; }
        /// <summary>
        /// Serialied XDocument
        /// </summary>
        public XDocument SerializedDocument { get; private set; }

        /// <summary>
        /// Creates instance of DefaultSerializer
        /// </summary>
        /// <param name="root"></param>
        public DefaultSerializer(DeserializedElement root)
        {
            Root = root;
        }

        /// <summary>
        /// Serializes the DeserializedElement tree using a given element name as a root.
        /// </summary>
        /// <param name="elementName">Root element name</param>
        /// <returns>Serialized XDocument</returns>
        public XDocument Serialize(string elementName)
        {
            IEnumerable<DeserializedElement> deserializedElements = Find(elementName);

            XElement root = new XElement("Root");
            foreach(var element in deserializedElements)
            {
                root.Add(SerializeElement(element));
            }

            XDocument document = new XDocument();

            document.Add(root);

            SerializedDocument = document;
            return SerializedDocument;
        }

        /// <summary>
        /// Serializes a single element.
        /// </summary>
        /// <param name="deserializedElement"></param>
        /// <returns>The serialized element</returns>
        public XElement SerializeElement(DeserializedElement deserializedElement)
        {
            XElement element = new XElement(deserializedElement.Name);
            
            foreach(var attribute in deserializedElement.Attributes)
            {
                element.SetAttributeValue(attribute.Key, attribute.Value);
            }


            foreach (var child in deserializedElement.Children)
            {
                element.Add(SerializeElement(child));
            }

            if (deserializedElement.Value != null)
            {
                element.Add(deserializedElement.Value);
            }



            return element;
        }

        /// <summary>
        /// Returns the collection of elements with the given elementName.
        /// </summary>
        /// <param name="elementName">Element name to search for.</param>
        /// <returns>Collection of elements of given element name.</returns>
        public IEnumerable<DeserializedElement> Find(string elementName)
        {
            List<DeserializedElement> elementList = new List<DeserializedElement>();

            TraverseTreeAndAddElementsByName(Root, elementList, elementName);

            return elementList;
        }

        /// <summary>
        /// Saves the document to file.
        /// </summary>
        /// <param name="path">File path</param>
        public void Save(string path)
        {
            if (SerializedDocument != null)
            {
                SerializedDocument.Save(path);
            }
        }

        /// <summary>
        /// Turns attributes into DeserializedElement children of the whole document.
        /// </summary>
        public void TurnAttributesIntoChildren()
        {
            if(Root != null) TurnAttributesIntoChildren(Root);
        }

        /// <summary>
        /// Turns attributes into DeserializedElement children of the element with the given name.
        /// </summary>
        /// <param name="targetElement">Element to search for</param>
        public void TurnAttributesIntoChildren(string targetElement)
        {
            foreach (var element in Find(targetElement))
            {
                element.TurnAttributesIntoChildren();
            }
        }

        /// <summary>
        /// Turns attributes into DeserializedElement children of the given element
        /// </summary>
        /// <param name="element">The given element</param>
        public void TurnAttributesIntoChildren(DeserializedElement element)
        {
            element.TurnAttributesIntoChildren();

            foreach(var child in element.Children)
            {
                TurnAttributesIntoChildren(child);
            }
        }

        /// <summary>
        /// Turns value into DeserializedElement child of the element with the given name.
        /// </summary>
        /// <param name="targetElement">Name of the given element</param>
        public void TurnValuesIntoChildren(string targetElement)
        {
            foreach(var element in Find(targetElement))
            {
                element.TurnValueIntoChild();
            }
        }

        private void TraverseTreeAndAddElementsByName(DeserializedElement element, List<DeserializedElement> list, string elementName)
        {
            if (element.Name == elementName) list.Add(element);
            
            foreach(var child in element.Children)
            {
                TraverseTreeAndAddElementsByName(child, list, elementName);
            }
        }
    }
}

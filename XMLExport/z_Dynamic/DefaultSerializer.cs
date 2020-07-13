﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace XMLExport
{
    public class DefaultSerializer
    {
        public DeserializedElement Root { get; private set; }
        public XDocument SerializedDocument { get; private set; }

        public DefaultSerializer(DeserializedElement root)
        {
            Root = root;
        }

        public void Serialize(string elementName)
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
        }

        public XElement SerializeElement(DeserializedElement deserializedElement)
        {
            XElement element = new XElement(deserializedElement.Name);
            
            foreach(var attribute in deserializedElement.Attributes)
            {
                element.SetAttributeValue(attribute.Key, attribute.Value);
            }

            if (deserializedElement.Value != null)
            {
                element.Value = deserializedElement.Value; 
            }

            foreach(var child in deserializedElement.Children)
            {
                element.Add(SerializeElement(child));
            }

            return element;
        }

        public IEnumerable<DeserializedElement> Find(string elementName)
        {
            List<DeserializedElement> elementList = new List<DeserializedElement>();

            TraverseTreeAndAddElementsByName(Root, elementList, elementName);

            return elementList;
        }

        public void Save(string path)
        {
            if (SerializedDocument != null)
            {
                SerializedDocument.Save(path);
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
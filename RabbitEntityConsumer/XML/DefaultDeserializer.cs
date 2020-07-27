using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace RabbitEntityConsumer
{
    public class DefaultDeserializer
    {
        public XDocument Document { get; private set; }
        public DeserializedElement Root { get; private set; }

        public DefaultDeserializer(XDocument document)
        {
            Document = document;
        }

        public DefaultDeserializer(string path)
        {
            Document = XDocument.Load(path);
        }

        public DefaultDeserializer(Stream stream)
        {
            Document = XDocument.Load(stream);
        }

        public void Deserialize()
        {
            XElement root = Document.Root;

            Root = new DeserializedElement(root);

        }

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

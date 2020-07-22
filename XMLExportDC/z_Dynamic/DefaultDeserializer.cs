using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace XMLExportDC
{
    public class DefaultDeserializer
    {
        public XDocument Document { get; private set; }
        public DeserializedElement Root { get; private set; }

        public DefaultDeserializer(XDocument document)
        {
            Document = document;
        }

        public void Deserialize()
        {
            XElement root = Document.Root;

            Root = new DeserializedElement(root);

        }
    }
}

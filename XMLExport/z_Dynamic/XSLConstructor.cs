using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace XMLExport
{
    public class XSLConstructor
    {
        public XDocument Document { get; private set; } = new XDocument();
        private readonly XNamespace xslNS = "xsl";

        public XSLConstructor()
        {
            XNamespace xslDeclaration = "http://www.w3.org/1999/XSL/Transform";

            XElement header = new XElement("stylesheet",
                new XAttribute(XNamespace.Xmlns + "xsl", xslDeclaration));


            //header.SetAttributeValue(XNamespace.Xmlns + "xsl", xslDeclaration);
            //header.SetAttributeValue("version", "1.0");

            Document.Add(header);

            XElement outputMethod = new XElement(xslNS + "output");
            outputMethod.SetAttributeValue("method", "text");
            outputMethod.SetAttributeValue("omit-xml-declaration", "yes");

            header.Add(outputMethod);

            
        }


        public void Save(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                Document.Save(writer);
            }


            Document.Save(path);
        }

    }
}

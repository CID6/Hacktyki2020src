using System;
using System.Collections;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace XMLExport
{

    class Program
    {
        static void Main(string[] args)
        {
            Dynamic();

            Console.ReadLine();
        }

        static void Dynamic()
        {
            XDocument xml = XDocument.Load(Resource.XMLTemplate);

            DefaultDeserializer deserializer = new DefaultDeserializer(xml);
            deserializer.Deserialize();


            Console.WriteLine();

            DefaultSerializer serializer = new DefaultSerializer(deserializer.Root);
            serializer.Serialize("Feature");
            serializer.Save(@"C:\Users\Czarek\Desktop\carXml2.xml");


            Console.ReadLine();
        }

        static void Hardcoded()
        {
            XDocument xml = XDocument.Load(Resource.XMLTemplate);

            //TestCodeFromSO();
            //PrintXMLDocumentNames(xml);
            ProductionReport report = ProductionReport.Deserialize(xml);
            Console.WriteLine("report done");

            Serializer serializer = new Serializer();

            serializer.SerializeCars(report);
            serializer.SaveToFile(@"C:\Users\Czarek\Desktop\carXml.xml");

            serializer.TransformToCSV(@"C:\Users\Czarek\Desktop\carXml.xml", @"C:\Users\Czarek\Desktop\carCSV.csv", Resource.XSLFile);

            Console.ReadLine();
        }

        static void PrintXMLDocumentNames(XDocument xml)
        {
            Console.WriteLine(xml.Root.Name.LocalName);

            PrintXMLNodeNames(xml.Root);
        }

        static void PrintXMLDocumentNames(XElement xml)
        {
            var p = xml;

            var textNodes = from c in p.Nodes()
                            where c.NodeType == XmlNodeType.Text
                            select (XText)c;

            //PrintXMLElementNames(xml.Root);
        }

        static void PrintXMLElementNames(XElement _element)
        {
            IEnumerable elements = _element.Elements();
            foreach (XElement element in elements)
            {
                Console.WriteLine(element.Name.LocalName);
                Console.WriteLine(element.FirstAttribute);
                if (element.NodeType == XmlNodeType.Text)
                {
                    Console.WriteLine("Text type in if");
                }
                Console.WriteLine(element.Value);
                PrintXMLElementNames(element);
            }
        }

        static void PrintXMLNodeNames(XElement _element)
        {
            IEnumerable elements = _element.Elements();
            foreach (XElement element in elements)
            {
                Console.WriteLine(element.Name.LocalName);
                Console.WriteLine(element.FirstAttribute);
                var textNodes = element.Nodes().OfType<XText>();
                foreach (var item in textNodes)
                {
                    Console.WriteLine(item.Value + "text value");
                }

                PrintXMLNodeNames(element);
            }
        }

        static void TestCodeFromSO()
        {
            var p = XElement.Parse("<parent>Hello<child>test1</child>World<child>test2</child>!</parent>");

            var textNodes = from c in p.Nodes()
                            where c.NodeType == XmlNodeType.Text
                            select (XText)c;

            foreach (var t in textNodes)
            {
                Console.WriteLine(t.Value);
            }
        }
    }
}

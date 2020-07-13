using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using XMLExport;
using System.Linq;
using System.Collections.Generic;

namespace XMLExportTests
{
    [TestClass]
    public class DefaultDeserializeTests
    {
        private const string testXMLPath = @"C://Users/Czarek/Desktop/testXML.xml";

        [TestMethod]
        public void TestNames()
        {
            XDocument document = XDocument.Load(testXMLPath);
            DefaultDeserializer testDeserializer = new DefaultDeserializer(document);

            testDeserializer.Deserialize();
            DeserializedElement root = testDeserializer.Root;

            Assert.AreEqual(root.Name, "Root");
            Assert.AreEqual(root.Children[0].Name, "Child1");
            Assert.AreEqual(root.Children[0].Children[0].Name, "ElementAsValue");
        }

        [TestMethod]
        public void TestAttributes()
        {
            XDocument document = XDocument.Load(testXMLPath);
            DefaultDeserializer testDeserializer = new DefaultDeserializer(document);

            testDeserializer.Deserialize();
            DeserializedElement root = testDeserializer.Root;

            Assert.AreEqual(root.Attributes["att1"], "val1");
            Assert.AreEqual(root.Attributes["att2"], "val2");
            Assert.AreEqual(root.Attributes["att3"], "val3");
        }

        [TestMethod]
        public void TestValues()
        {
            XDocument document = XDocument.Load(testXMLPath);
            DefaultDeserializer testDeserializer = new DefaultDeserializer(document);

            testDeserializer.Deserialize();
            DeserializedElement root = testDeserializer.Root;

            Assert.AreEqual(root.Value, "RootTextValue");
        }
    }
}

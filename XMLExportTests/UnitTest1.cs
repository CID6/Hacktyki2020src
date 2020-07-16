using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using XMLExport;
using System.Linq;
using System.Collections.Generic;

namespace XMLExportTests
{
    [TestClass]
    public class DefaultDeserializerTests
    {
        private const string testXMLPath = @"C://Users/Czarek/Desktop/testXML.xml";

        [TestMethod]
        public void Deserializer_SetName()
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
        public void Deserializer_SetValue()
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
        public void Deserializer_SetAttributes()
        {
            XDocument document = XDocument.Load(testXMLPath);
            DefaultDeserializer testDeserializer = new DefaultDeserializer(document);

            testDeserializer.Deserialize();
            DeserializedElement root = testDeserializer.Root;

            Assert.AreEqual(root.Value, "RootTextValue");
        }

        [TestMethod]
        public void TurnAttributesIntoChildren_FewAttributes_TurnsCorrectly()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("Att1", "Val1");
            attributes.Add("VIN", "1234");
            DeserializedElement testElement = new DeserializedElement(name: "Test element", attributes: attributes);

            testElement.TurnAttributesIntoChildren();

            Assert.AreEqual(2, testElement.Children.Count);
            Assert.AreEqual(0, testElement.Attributes.Count);
            Assert.AreEqual("Att1", testElement.Children.First().Name);
            Assert.AreEqual("Val1", testElement.Children.First().Value);
            Assert.AreEqual("VIN", testElement.Children.Last().Name);
            Assert.AreEqual("1234", testElement.Children.Last().Value);
        }

    }

    [TestClass]
    public class DefaultSerializerTests
    {
        private const string testXMLPath = @"C://Users/Czarek/Desktop/testXML.xml";

        [TestMethod]
        public void Find_Test()
        {
            XDocument document = XDocument.Load(testXMLPath);
            DefaultDeserializer testDeserializer = new DefaultDeserializer(document);
            testDeserializer.Deserialize();

            DefaultSerializer serializer = new DefaultSerializer(testDeserializer.Root);

            Assert.AreEqual("VVVVVV", serializer.Find("ElementAsValue").First().Value);
        }

        [TestMethod]
        public void SerializeElement_OnElement_ReturnsProper()
        {
            DeserializedElement testElement = new DeserializedElement(name: "testname", value: "test value");
            DefaultSerializer testSerializer = new DefaultSerializer(testElement);

            var output = testSerializer.SerializeElement(testElement);

            Assert.AreEqual("<testname>test value</testname>", output.ToString());
        }
    }

    [TestClass]
    public class ArgumentParsingTests
    {
        string inputPath = @"C:\Users\Czarek\source\repos\XMLExport\XMLExport\XMLs\template.xml";

        [TestMethod]
        public void CheckArgsValidity_IncorrectArgCount_Returns1()
        {
            string[] noArgs = { };
            string[] oneArg = { "inputPath" };
            string[] twoArgs = { "inputPath", "outputPath" };

            int noArgsResult = Program.CheckArgsValidity(noArgs);
            int oneArgResult = Program.CheckArgsValidity(oneArg);
            int twoArgsResult = Program.CheckArgsValidity(twoArgs);

            Assert.AreEqual(1, noArgsResult);
            Assert.AreEqual(1, oneArgResult);
            Assert.AreEqual(1, twoArgsResult);
        }

        [TestMethod]
        public void CheckArgsValidity_CorrectArgCount_Returns0()
        {
            string[] threeArgs = { "inputPath", "outputPath" , "column"};

            int threeArgsResult = Program.CheckArgsValidity(threeArgs);

            Assert.AreEqual(0, threeArgsResult);
        }

        [TestMethod]
        public void PrepareOutputPath_FileAlreadyExists_ReturnsFile()
        {
            string outputPath = @"E:\_test\asd.xml";

            string result = Program.PrepareOutputhPath(inputPath, outputPath);

            Assert.AreEqual(@"E:\_test\asd.xml", result);
        }

        [TestMethod]
        public void PrepareOutputPath_FileDoesntExist_ReturnsFile()
        {
            string outputPath = @"E:\_test\asd2.xml";

            string result = Program.PrepareOutputhPath(inputPath, outputPath);

            Assert.AreEqual(@"E:\_test\asd2.xml", result);
        }

        [TestMethod]
        public void PrepareOutputPath_OutputIsDirectoryWithBackslash_ReturnsFile()
        {
            string outputPath = @"E:\_test\";

            string result = Program.PrepareOutputhPath(inputPath, outputPath);

            Assert.AreEqual(@"E:\_test\template_output.xml", result);
        }

        [TestMethod]
        public void PrepareOutputPath_OutputIsDirectoryWithoutBackslash_ReturnsFile()
        {
            string outputPath = @"E:\_test";

            string result = Program.PrepareOutputhPath(inputPath, outputPath);

            Assert.AreEqual(@"E:\_test\template_output.xml", result);
        }

        [TestMethod]
        public void PrepareOutputPath_OutputIsNonexistingDirectory_ReturnsFile() //wyrzuci pozniej wyjatkiem
        {
            string outputPath = @"E:\_test\nonexistingdirectory\anotherone\";

            string result = Program.PrepareOutputhPath(inputPath, outputPath);

            Assert.AreEqual(@"E:\_test\nonexistingdirectory\anotherone\", result);
        }


    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XMLExportDC;

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

        [TestMethod]
        public void TurnArgsIntoChildren_Test()
        {
            Dictionary<string, string> testDict = new Dictionary<string, string>();
            testDict.Add("Att", "Value");
            DeserializedElement testElement = new DeserializedElement(name: "element", attributes: testDict);

            testElement.TurnAttributesIntoChildren();

            Assert.AreEqual("Value", testElement.FindChildren("Att").First().Value);
        }

        [TestMethod]
        public void FindChildren_Test()
        {
            XDocument document = XDocument.Load(testXMLPath);
            DefaultDeserializer testDeserializer = new DefaultDeserializer(document);
            testDeserializer.Deserialize();

            DeserializedElement testElement = testDeserializer.Root.FindChildren("Child1").First();

            Assert.AreEqual("bar", testElement.Attributes["foo"]);
        }
    }

    [TestClass]
    public class ArgumentParsingTests
    {
        [TestMethod]
        public void CheckArgsValidity_IncorrectArgCount_Returns1()
        {
            string[] noArgs = { };
            string[] oneArg = { "inputPath" };

            int noArgsResult = Program.CheckArgsValidity(noArgs);
            int oneArgResult = Program.CheckArgsValidity(oneArg);

            Assert.AreEqual(1, noArgsResult);
            Assert.AreEqual(1, oneArgResult);
        }

        [TestMethod]
        public void CheckArgsValidity_CorrectArgCount_Returns0()
        {
            string[] threeArgs = { "inputPath", "Car", "VIN" };

            int threeArgsResult = Program.CheckArgsValidity(threeArgs);

            Assert.AreEqual(0, threeArgsResult);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void OutputCSVOption_OptionWithoutArgument()
        {
            string[] args = { "inputPath", "Car", "-c" };

            Program.OutputCSVOption(args);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void OutputXMLOption_OptionWithoutArgument()
        {
            string[] args = { "inputPath", "Car", "-o" };

            Program.OutputXMLOption(args);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void OutputXSLOption_OptionWithoutArgument()
        {
            string[] args = { "inputPath", "Car", "-x" };

            Program.OutputXSLOption(args);
        }

        [TestMethod]
        public void OutputCSVOption_FolderGiven()
        {
            string[] args = { @"E:\_test\inputPath", "Car", "-c", @"E:\_test" };

            string response = Program.OutputCSVOption(args);

            Assert.AreEqual(@"E:\_test\inputPath_output.csv", response);
        }

        [TestMethod]
        public void OutputCSVOption_NonexistingFileGiven()
        {
            string[] args = { @"E:\_test\inputPath", "Car", "-c", @"E:\_test\nonexisting.jpg" };

            string response = Program.OutputCSVOption(args);

            Assert.AreEqual(@"E:\_test\nonexisting.jpg", response);
        }

        [TestMethod]
        public void OutputCSVOption_ExistingFileGiven()
        {
            string[] args = { @"E:\_test\inputPath", "Car", "-c", @"E:\_test\ProductionResults.xml" };

            string response = Program.OutputCSVOption(args);

            Assert.AreEqual(@"E:\_test\ProductionResults.xml", response);
        }

        [TestMethod]
        public void OutputXMLOption_FolderGiven()
        {
            string[] args = { @"E:\_test\inputPath", "Car", "-o", @"E:\_test" };

            string response = Program.OutputXMLOption(args);

            Assert.AreEqual(@"E:\_test\inputPath_serialized.xml", response);
        }

        [TestMethod]
        public void OutputXSLOption_FolderGiven()
        {
            string[] args = { @"E:\_test\inputPath", "Car", "-x", @"E:\_test" };

            string response = Program.OutputXSLOption(args);

            Assert.AreEqual(@"E:\_test\inputPath_template.xsl", response);
        }

        [TestMethod]
        public void ParseColumns_NoneGiven()
        {
            string[] args = { "inputPath", "Car", "-o", "path" };

            int count = Program.ParseColumns(args).Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ParseColumns_Given()
        {
            string[] args = { "inputPath", "Car", "col1", "col2", "-o", "path" };

            IEnumerable<string> output = Program.ParseColumns(args);

            Assert.AreEqual(2, output.Count());
            Assert.AreEqual("col1", output.ElementAt(0));
            Assert.AreEqual("col2", output.ElementAt(1));

        }



    }
}

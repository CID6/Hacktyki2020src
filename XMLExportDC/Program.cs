using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace XMLExportDC
{
    class Program
    {

        //argumenty
        //0 - input
        //1 - root to serialize
        //2n - columns
        //-o
        //-c

        const string usageMessage = "Usage: program input_path element_name {columns} [-o [output_xml_path]] [-c [output_csv_path]]";
        static int Main(string[] args)
        {

            int retArgs = CheckArgsValidity(args);
            if (retArgs != 0) return retArgs;

            string inputPath = args[0];
            string outputXMLPath = null;
            string outputCSVPath = null;
            string elementName = args[1];

            try
            {
                outputXMLPath = OutputXMLOption(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(usageMessage);
                return 100;
            }

            try
            {
                outputCSVPath = OutputCSVOption(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(usageMessage);

                return 101;
            }

            Stream transposedStream = null;

            try
            {
                transposedStream = TransposeXML(inputPath, outputXMLPath, elementName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(usageMessage);
                return 10;
            }

            List<string> columnNames = null;
            columnNames = new List<string>(ParseColumns(args));




            XSLConstructor constructor = new XSLConstructor(columnNames);

            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            constructor.ReadXSLTemplate(a.GetManifestResourceStream("XMLExportDC.XMLs.outputTemplate.xsl"));

            constructor.BuildTemplate();


            byte[] xslTemplateBytes = Encoding.UTF8.GetBytes(constructor.XSLTemplate);
            MemoryStream xslStream = new MemoryStream(xslTemplateBytes);

            //constructor.Save(@"E:\_test\xslSavepath.xsl");



            TransformToCSV(transposedStream, outputCSVPath, xslStream);

            return 0;
        }

        public static int CheckArgsValidity(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the input file path.");
                Console.WriteLine(usageMessage);
                return 1;
            }

            if (args.Length == 1)
            {
                Console.WriteLine("Please enter the element name.");
                Console.WriteLine(usageMessage);

                return 1;
            }

            return 0;
        }

        private static string OutputXMLOption(string[] args)
        {
            string outputXMLLocation = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-o")
                {
                    try
                    {
                        outputXMLLocation = args[i + 1];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Please input the output XML file's path.");
                        Console.WriteLine(usageMessage);
                        throw;
                    }
                }
            }

            if (outputXMLLocation != null)
            {
                outputXMLLocation = PrepareOutputhPath(args[0], outputXMLLocation, "xml");

                Console.WriteLine("Option -o was given. File will be saved at: {0}", outputXMLLocation);
            }

            return outputXMLLocation;

        }

        private static string OutputCSVOption(string[] args)
        {
            string outputCSVLocation = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-c")
                {
                    try
                    {
                        outputCSVLocation = args[i + 1];
                    }
                    catch
                    {
                        Console.WriteLine("Please input the output CSV file's path.");
                        Console.WriteLine(usageMessage);
                        throw;
                    }
                }
            }

            if (outputCSVLocation != null)
            {
                outputCSVLocation = PrepareOutputhPath(args[0], outputCSVLocation, "csv");

                Console.WriteLine("Option -c was given. CSV file will be saved at: {0}", outputCSVLocation);
            }
            else
            {
                outputCSVLocation = CreateDefaultOutputPath(args[0], "csv");

                Console.WriteLine("CSV output file path was not specified. File will be saved at: {0}", outputCSVLocation);
            }

            return outputCSVLocation;

        }

        static string CreateDefaultOutputPath(string inputPath, string extension)
        {
            string outputPath;

            string filename = Path.GetFileNameWithoutExtension(inputPath) + "_output";
            string directory = Path.GetDirectoryName(inputPath);
            outputPath = Path.Combine(directory, filename);
            outputPath = Path.ChangeExtension(outputPath, extension);

            return outputPath;
        }

        private static string PrepareOutputhPath(string inputPath, string outputPath, string extension)
        {
            string returnPath;
            if (!File.Exists(outputPath))
            {


                FileAttributes attributes = FileAttributes.Normal;
                try
                {
                    attributes = File.GetAttributes(outputPath);
                }
                catch (Exception e)
                {
                    //throw new Exception("File nor directory does not exist.");                   
                }

                if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {

                    //Console.WriteLine("file is a directory");
                    string filename = Path.GetFileNameWithoutExtension(inputPath) + "_output";
                    filename = Path.ChangeExtension(filename, extension);

                    returnPath = Path.Join(outputPath, filename);

                }
                else
                {
                    returnPath = outputPath;
                }
            }
            else
            {
                returnPath = outputPath;
            }

            return returnPath;
        }

        private static Stream TransposeXML(string inputPath, string outputPath, string columnName)
        {
            XDocument xml = XDocument.Load(inputPath);

            DefaultDeserializer deserializer = new DefaultDeserializer(xml);
            deserializer.Deserialize();


            DefaultSerializer serializer = new DefaultSerializer(deserializer.Root);
            serializer.TurnAttributesIntoChildren(columnName);
            serializer.Serialize(columnName);
            if (outputPath!=null)
            {
                serializer.Save(outputPath); 
            }

            Stream stream = new MemoryStream();
            serializer.SerializedDocument.Save(stream);
            stream.Position = 0;

            return stream;
        }

        private static IEnumerable<string> ParseColumns(string[] args)
        {
            int i = 2;
            List<string> columnList = new List<string>();
            while(i <= args.Length - 1 && args[i]!="-o" && args[i] != "-c")
            {
                columnList.Add(args[i]);
                i++;
            }

            return columnList;
        }

        private static void TransformToCSV(Stream inputXMLStream, string outputCSVpath, Stream xslStream)
        {

            XsltSettings settings = new XsltSettings(true, true);
            settings.EnableScript = true;
            settings.EnableDocumentFunction = true;

            XslCompiledTransform xslt = new XslCompiledTransform();

            XmlReader reader = XmlReader.Create(xslStream);
            xslt.Load(stylesheet: reader, settings: settings, stylesheetResolver: new XmlUrlResolver());

            XmlReader inputReader = XmlReader.Create(inputXMLStream);
            Stream stream = new MemoryStream();
            xslt.Transform(input: inputReader, arguments: null, results: new FileStream(outputCSVpath, FileMode.OpenOrCreate, FileAccess.Write));

        }
    }
}

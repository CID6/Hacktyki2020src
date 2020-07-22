using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace XMLExportHC
{
    class Program
    {

        const string usageMessage = "Usage: program input_path [-o [output_xml_path]] [-c [output_csv_path]]";

        static int Main(string[] args)
        {
            

            int retArgs = CheckArgsValidity(args);
            if (retArgs != 0) return retArgs;

            string inputPath = args[0];

            string outputXMLPath = null;

            string outputCSVPath = null;

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
                transposedStream = TransposeXML(inputPath, outputXMLPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(usageMessage);
                return 10;
            }

            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            Stream templateStream = a.GetManifestResourceStream("XMLExportHC.XMLs.output3.xsl");

            TransformToCSV(transposedStream, outputCSVPath, templateStream);

            Console.ReadLine();

            return 0;
        }

        static int CheckArgsValidity(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the input file path.");
                Console.WriteLine(usageMessage);
                return 1;
            }

            return 0;
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

        private static Stream TransposeXML(string inputPath, string outputPath)
        {
            XDocument xml = XDocument.Load(inputPath);

            ProductionReport report = ProductionReport.Deserialize(xml);


            Serializer serializer = new Serializer();

            serializer.SerializeCars(report);

            if (outputPath!=null)
            {
                serializer.SaveToFile(outputPath); 
            }

            Stream stream = new MemoryStream();
            serializer.CarDocument.Save(stream);
            stream.Position = 0;

            return stream;    
        }
        

        private static void TransformToCSV(string inputXMLpath, string outputCSVpath, Stream xslStream)
        {

            XsltSettings settings = new XsltSettings(true, true);
            settings.EnableScript = true;
            settings.EnableDocumentFunction = true;

            XslCompiledTransform xslt = new XslCompiledTransform();

            XmlReader reader = XmlReader.Create(xslStream);
            xslt.Load(stylesheet: reader,settings: settings,stylesheetResolver: new XmlUrlResolver());

            xslt.Transform(inputXMLpath, outputCSVpath);

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
                if(args[i] == "-c")
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
    }
}

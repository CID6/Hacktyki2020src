using System;
using System.Collections;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace XMLExport
{

    public class Program
    {
        //0 - input path
        //1 - output path
        //2 - column
        static int Main(string[] args)
        {
            XSLConstructor();


            Console.ReadLine();

            return 0;
        }

        static void XSLConstructor()
        {
            string[] columns = { "ProductionYear", "VIN", "Model" };
            XSLConstructor constructor = new XSLConstructor(columns);

            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            constructor.ReadXSLTemplate(a.GetManifestResourceStream("XMLExport.XMLs.outputTemplate.xsl"));

            constructor.BuildTemplate();
            constructor.Save(@"E:\_test\xslSavepath.xsl");
        }

        static int MAIN2(string[] args)
        {
            int retArgs = CheckArgsValidity(args);
            if (retArgs != 0) return retArgs;

            string inputPath = args[0];
            string outputPath = null;
            string columnName = null;

            if (args.Length >= 3)
            {
                try
                {
                    outputPath = PrepareOutputhPath(inputPath, args[1]);
                    columnName = args[2];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 99;
                }
            }

            Console.WriteLine("File will be saved to: " + outputPath);


            try
            {
                TransposeXML(inputPath, outputPath, columnName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 10;
            }

            return 0;
        }

        public static int CheckArgsValidity(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the input file path.");
                Console.WriteLine("Usage: program input_path output_path column");
                return 1;
            }

            if (args.Length == 1)
            {
                Console.WriteLine("Please enter the output file path.");
                Console.WriteLine("Usage: program input_path output_path column");

                return 1;
            }

            if (args.Length == 2)
            {
                Console.WriteLine("Please enter the column name.");
                Console.WriteLine("Usage: program input_path output_path column");

                return 1;
            }

            return 0;
        }


        public static string PrepareOutputhPath(string inputPath, string outputPath)
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
                    filename = Path.ChangeExtension(filename, "xml");

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

        private static void TransposeXML(string inputPath, string outputPath, string columnName)
        {
            XDocument xml = XDocument.Load(inputPath);

            DefaultDeserializer deserializer = new DefaultDeserializer(xml);
            deserializer.Deserialize();


            DefaultSerializer serializer = new DefaultSerializer(deserializer.Root);
            serializer.TurnAttributesIntoChildren(columnName);
            serializer.Serialize(columnName);
            serializer.Save(outputPath);
        }

    }
}

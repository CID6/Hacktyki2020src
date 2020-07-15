using System;
using System.IO;
using System.Xml.Linq;

namespace XMLExportH
{
    class Program
    {
        //0 - input path
        //1 - outputPath
        static int Main(string[] args)
        {
            int retArgs = CheckArgsValidity(args);
            if (retArgs != 0) return retArgs;

            string inputPath = args[0];
            string outputPath = null;

            if (args.Length == 1)
            {
                Console.WriteLine("Output path was not given. Output .csv file will be saved in input file's directory.");

                outputPath = CreateDefaultOutputPath(inputPath);
            }

            if (args.Length >= 2)
            {
                try
                {
                    outputPath = PrepareOutputhPath(inputPath, args[1]);
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
                TransposeXML(inputPath, outputPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 10;
            }

            return 0;
        }

        static int CheckArgsValidity(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the input file path.");
                Console.WriteLine("Usage: program input_path [output_path]");
                return 1;
            }

            return 0;
        }

        static string CreateDefaultOutputPath(string inputPath)
        {
            string outputPath;

            string filename = Path.GetFileNameWithoutExtension(inputPath) + "_output";
            string directory = Path.GetDirectoryName(inputPath);
            outputPath = Path.Combine(directory, filename);
            outputPath = Path.ChangeExtension(outputPath, "xml");

            return outputPath;
        }

        private static string PrepareOutputhPath(string inputPath, string outputPath)
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

        private static void TransposeXML(string inputPath, string outputPath)
        {
            XDocument xml = XDocument.Load(inputPath);

            ProductionReport report = ProductionReport.Deserialize(xml);


            Serializer serializer = new Serializer();

            serializer.SerializeCars(report);
            serializer.SaveToFile(outputPath);

        }
    }
}

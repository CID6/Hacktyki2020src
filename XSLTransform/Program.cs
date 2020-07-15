using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace XSLTransform
{
    class Program
    {
        //0 - xml path
        //1 - xsl path
        //2 - ouput path, optional
        //
        static int Main(string[] args)
        {

            int retArgs = CheckArgsValidity(args);
            if (retArgs != 0) return retArgs;


            //if (CheckPathValidity(args) == 1) return 3;

            string inputPath = args[0];
            string xslPath = args[1];
            string outputPath = null;

            if(args.Length == 2)
            {
                Console.WriteLine("Output path was not given. Output .csv file will be saved in input file's directory.");
                outputPath = Path.ChangeExtension(inputPath, "csv");
            }

            if(args.Length >= 3 && args[2]!="-c")
            {
                if (!File.Exists(args[2]))
                {
                    outputPath = args[2];
                }
                else
                {
                    FileAttributes attributes = File.GetAttributes(args[2]);
                    if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        string filename = Path.ChangeExtension(inputPath, "csv");
                        filename = Path.GetFileName(filename);

                        outputPath = Path.Join(args[2], filename);

                    }
                    else outputPath = args[2]; 
                }
            }

            Console.WriteLine("output path: " + outputPath);
            try
            {
                TransformToCSV(inputPath, outputPath, xslPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 10;
            }

            Console.WriteLine("Hello World!");

            Console.ReadLine();

            return 0;
        }

        static int CheckArgsValidity(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the input file path.");
                Console.WriteLine("Usage: program input_path xsl_path [output_path] [-c :columns_to_convert:]");
                return 1;
            }

            if (args.Length == 1)
            {
                Console.WriteLine("Please enter the XSLT file path.");
                Console.WriteLine("Usage: program input_path xsl_path [output_path] [-c :columns_to_convert:]");
                return 2;
            }

            return 0;
        }

        static int CheckPathValidity(string[] args)
        {
            if(!(Uri.IsWellFormedUriString(args[0], UriKind.RelativeOrAbsolute)))
            {
                Console.WriteLine("Incorrect input file path.");
                Console.WriteLine("Usage: program input_path xsl_path [output_path] [-c :columns_to_convert:]");
                return 1;
            }

            if (!(Uri.IsWellFormedUriString(args[1], UriKind.RelativeOrAbsolute)))
            {
                Console.WriteLine("Incorrect XSLT file path.");
                Console.WriteLine("Usage: program input_path xsl_path [output_path] [-c :columns_to_convert:]");
                return 1;
            }

            return 0;
        }

        private static void TransformToCSV(string inputXMLpath, string outputCSVpath, string xslPath)
        {

            XsltSettings settings = new XsltSettings(true, true);
            settings.EnableScript = true;
            settings.EnableDocumentFunction = true;

            XslCompiledTransform xslt = new XslCompiledTransform();

            xslt.Load(xslPath, settings, new XmlUrlResolver());

            xslt.Transform(inputXMLpath, outputCSVpath);

        }
    }
}

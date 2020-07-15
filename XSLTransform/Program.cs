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
            //are there enough arguments?
            int retArgs = CheckArgsValidity(args);
            if (retArgs != 0) return retArgs;

            //im not checking path validity, the application throws an exception if thats the case later.
        
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
                PrepareOutputhPath(inputPath, args[2]);
            }

            Console.WriteLine("File will be saved to: " + outputPath);

            try
            {
                TransformToCSV(inputPath, outputPath, xslPath);
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
                Console.WriteLine("Usage: program input_path xsl_path [output_path]");
                return 1;
            }

            if (args.Length == 1)
            {
                Console.WriteLine("Please enter the XSLT file path.");
                Console.WriteLine("Usage: program input_path xsl_path [output_path]");
                return 2;
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

        private static string PrepareOutputhPath(string inputPath, string outputPath)
        {
            string returnPath;
            if (!File.Exists(outputPath))
            {
                returnPath = outputPath;
            }
            else
            {
                FileAttributes attributes = File.GetAttributes(outputPath);
                if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    string filename = Path.ChangeExtension(inputPath, "csv");
                    filename = Path.GetFileName(filename);

                    returnPath = Path.Join(outputPath, filename);

                }
                else returnPath = outputPath;
            }

            return returnPath;
        }

        
    }
}

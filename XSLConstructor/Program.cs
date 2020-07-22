using System;
using System.IO;

namespace XSLConstructor
{
    class Program
    {
        static int Main(string[] args)
        {
            int retArgs = CheckArgsValidity(args);
            if (retArgs != 0) return retArgs;

            string[] columns = null;
            string outputPath = null;


            if (args.Length >= 2)
            {
                columns = new string[args.Length - 1];

                try
                {
                    outputPath = PrepareOutputhPath(args[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 99;
                }
            }

            //fit columns
            for (int i = 1; i < args.Length; i++)
            {
                columns[i - 1] = args[i];
            }

            ConstructXSL(columns, outputPath);
            

            Console.WriteLine("Hello World!");
            return 0;
        }


        static int CheckArgsValidity(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the output file path.");
                Console.WriteLine("Usage: program output_path {column_names}");
                return 1;
            }

            if(args.Length == 1)
            {
                Console.WriteLine("Please enter the column names");
                Console.WriteLine("Usage: program output_path {column_names}");
                return 1;
            }

            return 0;
        }


        private static string PrepareOutputhPath(string outputPath)
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
                    string filename = "output";
                    filename = Path.ChangeExtension(filename, "xsl");

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

        static void ConstructXSL(string[] columns, string outputPath)
        {
            XSLConstructor constructor = new XSLConstructor(columns);

            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            constructor.ReadXSLTemplate(a.GetManifestResourceStream("XSLConstructor.XMLs.outputTemplate.xsl"));

            constructor.BuildTemplate();
            constructor.Save(outputPath);

        }
    }
}

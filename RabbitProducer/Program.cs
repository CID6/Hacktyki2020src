using System;
using System.IO;

namespace RabbitProducer
{
    class Program
    {
        const string usageString = "Usage: program input_xml_file";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(usageString);
                return;
            }

            var rpcClient = new RpcClient();

            string fileText = "";
            try
            {
                fileText = File.ReadAllText(args[0]);
            }
            catch (Exception)
            {
                Console.WriteLine("Incorrect input file.");
                return;
            }

            Console.WriteLine(" [x] Sending file {0}", args[0]);
            string response = rpcClient.Call(fileText);

            Console.WriteLine(" [.] Got '{0}'", response);
            rpcClient.Close();

            Console.ReadLine();
        }

    }
}

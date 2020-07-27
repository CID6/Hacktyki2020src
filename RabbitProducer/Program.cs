using Microsoft.VisualBasic.CompilerServices;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Text;

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

            Console.WriteLine(" [x] Sending file {0}", args[0]);
            string response = rpcClient.Call(File.ReadAllText(args[0]));

            Console.WriteLine(" [.] Got '{0}'", response);
            rpcClient.Close();

            Console.ReadLine();
        }

    }
}

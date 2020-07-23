using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitSend
{
    class Program
    {
        static void Main(string[] args)
        {
            Main2(args);

            Console.ReadLine();
        }


        static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? args[0] : "1");
        }

        static void Main1(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "hello",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);


                    string message = GetMessage(args);
                    byte[] body = Encoding.UTF8.GetBytes(message);

                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: properties, body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit");
            Console.ReadLine();
        }

        public static void Main2(string[] args)
        {
            var rpcClient = new RpcClient();

            Console.WriteLine(" [x] Requesting fib({0})", GetMessage(args));
            var response = rpcClient.Call(GetMessage(args));

            Console.WriteLine(" [.] Got '{0}'", response);
            rpcClient.Close();
        }

    }
}

using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.IO;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace RabbitReceiveFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Main2();
        }

        static void Przetwarzanie()
        {
            Console.WriteLine("Przetwarzanie...");
        }

        static void Main2()
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpc_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    string response = null;

                    byte[] body = ea.Body.ToArray();
                    IBasicProperties props = ea.BasicProperties;
                    IBasicProperties replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        Console.WriteLine("Received report");
                        MemoryStream stream = new MemoryStream(body);
                        AddToDatabase(stream);
                        Console.WriteLine("Report added to the database");
                        response = "Report added to the database.";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    }
                    finally
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static int fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return fib(n - 1) + fib(n - 2);
        }

        private static void AddToDatabase(Stream inputStream)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
                @"Data Source=(LocalDb)\MSSQLLocalDB;" +
                "Integrated Security=true;" +
                "Database=CarsDB;";

            connection.Open();

            Stream xmlStream = inputStream;

            SqlCommand insertXMLCommand = new SqlCommand();
            insertXMLCommand.CommandText =
                "INSERT INTO XMLwithOpenXML(XMLData, LoadedDateTime)" +
                "SELECT @xmlstring, GETDATE()";
            insertXMLCommand.Connection = connection;


            insertXMLCommand.Parameters.Add(new SqlParameter("@xmlstring", System.Data.SqlDbType.Xml)
            {
                Value = new SqlXml(xmlStream)
            });
            insertXMLCommand.ExecuteNonQuery();

            SqlCommand insertCarProducts = new SqlCommand();
            insertCarProducts.CommandText =
                "DECLARE @XML AS XML, @hDoc AS INT, @SQL NVARCHAR (MAX)" + "\n\r" +
                "SELECT @XML = XMLData FROM XMLwithOpenXML" + "\n\r" +
                "EXEC sp_xml_preparedocument @hDoc OUTPUT, @XML" + "\n\r" +
                "INSERT INTO CarProducts([Year], [VIN], [CarModelId], [FactoryId])" + "\n\r" +
                "SELECT ProductionYear as 'Year', VIN, CarModels.Id as CarModelId, CarFactories.Id as FactoryId" + "\n\r" +
                "FROM OPENXML(@hDoc, 'ProductionReport/Factories/Factory/ProducedCars/Car')" + "\n\r" +
                "WITH" + "\n\r" +
                "(" + "\n\r" +
                "VIN [varchar](20) '@VIN'," + "\n\r" +
                "ProductionYear varchar(4) 'ProductionYear'," + "\n\r" +
                "Model [varchar](100) 'Model'," + "\n\r" +
                "Manufacturer [varchar](50) '../../../../@Manufacturer'," + "\n\r" +
                "FactoryName [varchar](100) '../../@Name'" + "\n\r" +
                ")" + "\n\r" +
                "INNER JOIN CarModels ON Model = CarModels.Name" + "\n\r" +
                "INNER JOIN CarFactories ON FactoryName = CarFactories.Name" + "\n\r" +
                "EXEC sp_xml_removedocument @hDoc;" + "\n\r";
            insertCarProducts.Connection = connection;
            insertCarProducts.ExecuteNonQuery();

            SqlCommand insertCarProductsCarFeatures = new SqlCommand();
            insertCarProductsCarFeatures.CommandText =
                "DECLARE @XML AS XML, @hDoc AS INT, @SQL NVARCHAR (MAX)" + "\n\r" +
                "SELECT @XML = XMLData FROM XMLwithOpenXML" + "\n\r" +
                "EXEC sp_xml_preparedocument @hDoc OUTPUT, @XML" + "\n\r" +
                "INSERT INTO CarProductCarFeature(CarProductId, InstalledFeatureId)" + "\n\r" +
                "SELECT dbo.CarProducts.Id as CarID, dbo.CarFeatures.Id as FeatureId" + "\n\r" +
                "FROM OPENXML(@hDoc, 'ProductionReport/Factories/Factory/ProducedCars/Car/Features/Feature')" + "\n\r" +
                "WITH" + "\n\r" +
                "(" + "\n\r" +
                "VIN [varchar](20) '../../@VIN'," + "\n\r" +
                "Code[varchar](4) '@Code'" + "\n\r" +
                ") AS carXML" + "\n\r" +
                "INNER JOIN CarFeatures ON carXML.Code = CarFeatures.Code" + "\n\r" +
                "INNER JOIN CarProducts ON carXML.VIN = CarProducts.VIN" + "\n\r" +
                "EXEC sp_xml_removedocument @hDoc" + "\n\r";
            insertCarProductsCarFeatures.Connection = connection;
            insertCarProductsCarFeatures.ExecuteNonQuery();

            connection.Close();
        }
    }
}

using Microsoft.EntityFrameworkCore.Internal;
using RabbitEntityConsumer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace RabbitEntityConsumer
{
    class Program
    {
        static void Main(string[] args)
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
                        Console.WriteLine(" [.] Received report");
                        MemoryStream stream = new MemoryStream(body);
                        AddToDatabase(stream);
                        Console.WriteLine(" [.] Report added to the database");
                        response = "Report added to the database.";
                    }
                    catch (Exception e)
                    {
                        string exMessage = e.InnerException.Message;
                        Console.WriteLine(" [.] " + exMessage);
                        response = "An error has occured: " + exMessage;
                    }
                    finally
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                };

                Console.WriteLine(" Type [exit] to exit.");
                Console.ReadLine();

                
            }
        }

        static void AddToDatabase(Stream stream)
        {
            DefaultDeserializer deserializer = new DefaultDeserializer(stream);
            deserializer.Deserialize();

            CarsDBContext db = new CarsDBContext();

            //gets reference to added report for later
            Reports addedReport;

            try
            {
                addedReport = AddReport(db, deserializer.Document.ToString());
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                AddCarProducts(db, deserializer);
                db.SaveChanges();

                AddCarProductCarFeature(db, deserializer);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            ConfirmReport(context: db, reportToConfirm: addedReport);
            db.SaveChanges();

        }
        
        static void AddCarProducts(CarsDBContext context, DefaultDeserializer deserializer)
        {
            var carModelsQuery = context.CarModels;
            var carFactoriesQuery = context.CarFactories;

            foreach (var factory in deserializer.Find("Factory"))
            {
                //finds factory id by correlating same factory name attribute and Name column in CarFactories
                int factoryID = carFactoriesQuery.Where(s => s.Name == factory.Attributes["Name"]).First().Id;
                foreach (var car in factory.FindChildren("Car"))
                {
                    int carID = carModelsQuery.Where(s => s.Name == car.FindChildren("Model").First().Value).First().Id;
                    short carYear = short.Parse(car.FindChildren("ProductionYear").First().Value);
                    string carVIN = car.Attributes["VIN"];
                    context.Add(new CarProducts { CarModelId = carID, Year = carYear, Vin = carVIN, FactoryId = factoryID });
                }
            }
        }


        static void AddCarProductCarFeature(CarsDBContext context, DefaultDeserializer deserializer)
        {
            var carProductsQuery = context.CarProducts;
            var carFeaturesQuery = context.CarFeatures;

            foreach (var car in deserializer.Find("Car"))
            {
                int carID = carProductsQuery.Where(s => s.Vin == car.Attributes["VIN"]).First().Id;
                foreach (var feature in car.FindChildren("Feature"))
                {
                    int featureID = carFeaturesQuery.Where(s => s.Code == feature.Attributes["Code"]).First().Id;
                    context.Add(new CarProductCarFeature { CarProductId = carID, InstalledFeatureId = featureID });
                }
            }
        }


        static Reports AddReport(CarsDBContext context, string xmlText)
        {
            Reports report = new Reports { ReportData = xmlText, RequestedDateTime = DateTime.Now, AddedToDatabase = false };
            context.Add(report);
            return report;
        }

        static void ConfirmReport(CarsDBContext context, Reports reportToConfirm)
        {
            reportToConfirm.AddedToDatabase = true;
        }
    }
}

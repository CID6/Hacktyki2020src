using Microsoft.EntityFrameworkCore.Internal;
using RabbitEntityConsumer.Models;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace RabbitEntityConsumer
{
    class Program
    {
        static void Main(string[] args)
        {




        }

        static void Main2(string[] args)
        {

            DefaultDeserializer deserializer = new DefaultDeserializer(args[0]);
            deserializer.Deserialize();

            var db = new CarsDBContext();


            var carModelsQuery = db.CarModels;
            var carFactoriesQuery = db.CarFactories;

            foreach(var factory in deserializer.Find("Factory"))
            {
                //finds factory id by correlating same factory name attribute and Name column in CarFactories
                int factoryID = carFactoriesQuery.Where(s => s.Name == factory.Attributes["Name"]).First().Id;
                foreach(var car in factory.FindChildren("Car"))
                {
                    int carID = carModelsQuery.Where(s => s.Name == car.FindChildren("Model").First().Value).First().Id;
                    short carYear = short.Parse(car.FindChildren("ProductionYear").First().Value);
                    string carVIN = car.Attributes["VIN"];
                    db.Add(new CarProducts { CarModelId = carID, Year = carYear, Vin = carVIN, FactoryId = factoryID });
                }
            }
            db.SaveChanges();

            var carProductsQuery = db.CarProducts;
            var carFeaturesQuery = db.CarFeatures;

            foreach(var car in deserializer.Find("Car"))
            {
                int carID = carProductsQuery.Where(s => s.Vin == car.Attributes["VIN"]).First().Id;
                foreach(var feature in car.FindChildren("Feature"))
                {
                    int featureID = carFeaturesQuery.Where(s => s.Code == feature.Attributes["Code"]).First().Id;
                    db.Add(new CarProductCarFeature { CarProductId = carID, InstalledFeatureId = featureID });
                }
            }


            db.SaveChanges();
        }
    }
}

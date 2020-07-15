using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XMLExportH
{
    public abstract class PRNode
    {
        public PRNode()
        {

        }
    }

    public class ProductionReport : PRNode
    {

        public string Manufacturer { get; set; }
        public DateTime Date { get; set; }

        public List<Factory> Factories { get; set; }

        public ProductionReport(XElement xElement)
        {
            foreach (var attribute in xElement.Attributes())
            {
                if (attribute.Name == "Manufacturer")
                {
                    Manufacturer = attribute.Value;
                }
                else if (attribute.Name == "Date")
                {
                    Date = DateTime.Parse(attribute.Value);
                }
            }


            Factories = new List<Factory>();
            XElement factoriesElement = xElement.Descendants().First();
            foreach (XElement factory in factoriesElement.Elements())
            {
                Factories.Add(new Factory(factory));
            }
        }


        public static ProductionReport Deserialize(XDocument xml)
        {
            return new ProductionReport(xml.Root);
        }
    }

    public class Factory : PRNode
    {
        public string Name { get; set; }
        public List<Car> ProducedCars { get; set; }

        public Factory(XElement xElement)
        {
            foreach (var attribute in xElement.Attributes())
            {
                if (attribute.Name == "Name")
                {
                    Name = attribute.Value;
                }
            }

            ProducedCars = new List<Car>();
            XElement carsElement = xElement.Descendants().First();
            foreach (XElement car in carsElement.Elements())
            {
                ProducedCars.Add(new Car(car));
            }
        }
    }

    public class Car : PRNode
    {
        public string VIN { get; set; }
        public int ProductionYear { get; set; }
        public string Model { get; set; }
        public List<Feature> Features { get; set; }

        public Car(XElement xElement)
        {
            foreach (var attribute in xElement.Attributes())
            {
                if (attribute.Name == "VIN")
                {
                    VIN = attribute.Value;
                }
            }

            foreach (var element in xElement.Elements())
            {
                if (element.Name.LocalName == "ProductionYear")
                {
                    ProductionYear = int.Parse(element.Value);
                }

                if (element.Name.LocalName == "Model")
                {
                    Model = element.Value;
                }

                if (element.Name.LocalName == "Features")
                {
                    Features = new List<Feature>();
                    foreach (XElement feature in element.Elements())
                    {
                        Features.Add(new Feature(feature));
                    }
                }
            }
        }

    }

    public class Feature : PRNode
    {
        public string Code { get; set; }
        public string FeatureName { get; set; }

        public Feature(XElement xElement)
        {
            foreach (var attribute in xElement.Attributes())
            {
                if (attribute.Name == "Code")
                {
                    Code = attribute.Value;
                }
            }

            FeatureName = xElement.Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace XMLExport
{
    public class Serializer
    {

        public List<Car> CarList { get; private set; }
        public XDocument CarDocument { get; private set; }

        public Serializer()
        {
            CarList = new List<Car>();
        }

        public void SerializeCars(ProductionReport report)
        {
            FillCarList(report);

            Console.WriteLine("fillcarlist finished");

            CreateXDocument();

            Console.WriteLine("createxdocfinished");
        }

        public void SaveToFile(string path)
        {
            CarDocument.Save(path);
        }

        public void TransformToCSV(string inputXMLpath, string outputCSVpath, string xslPath)
        {
            if (CarDocument == null) throw new Exception("Document is not created.");

            XsltSettings settings = new XsltSettings(true, true);
            settings.EnableScript = true;
            settings.EnableDocumentFunction = true;

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xslPath, settings, new XmlUrlResolver());
            
            
            xslt.Transform(inputXMLpath, outputCSVpath);

        }

        private void FillCarList(ProductionReport report)
        {
            foreach(Factory factory in report.Factories)
            {
                CarList.AddRange(factory.ProducedCars);
            }
        }

        private void CreateXDocument()
        {
            CarDocument = new XDocument();

            XElement root = new XElement("ProducedCars");
            foreach(Car car in CarList)
            {
                root.Add(CreateCarXElement(car));
            }

            CarDocument.Add(root);
        }

        private XElement CreateCarXElement(Car car)
        {
            XElement carXElement = new XElement("Car");
            carXElement.Add(new XElement("Model", car.Model));
            carXElement.Add(new XElement("ProductionYear", car.ProductionYear));
            carXElement.Add(new XElement("VIN", car.VIN));

            XElement features = new XElement("Features");
            foreach(Feature feature in car.Features)
            {
                XElement featureXElement = new XElement("Feature", feature.FeatureName);
                featureXElement.SetAttributeValue("Code", feature.Code);
                features.Add(featureXElement);
            }

            carXElement.Add(features);
            return carXElement;
        }

    }
}

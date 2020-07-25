using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data.SqlTypes;

namespace InsertDataIntoSQLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
                @"Data Source=(LocalDb)\MSSQLLocalDB;" +
                "Integrated Security=true;" +
                "Database=CarsDB;";

            connection.Open();

            SqlCommand command = new SqlCommand
            {
                CommandText = "select * from CarModels;",
                Connection = connection
            };

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine(String.Format("{0}, {1}",
                        reader[0], reader[1]));
                }
            }


            FileStream xmlStream = File.OpenRead(@"E:\_test\ProductionResults.xml");

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
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}

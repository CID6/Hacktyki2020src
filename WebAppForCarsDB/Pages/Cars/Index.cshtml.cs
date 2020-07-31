using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RabbitEntityConsumer.Models;
using RabbitMQ.Client.Events;
using WebAppForCarsDB.Hubs;

namespace WebAppForCarsDB.Pages.Cars
{
    public class IndexModel : PageModel
    {
        private readonly RabbitEntityConsumer.Models.CarsDBContext _context;

        SqlDataReader dependencyReader;
        SqlConnection connection;
        SqlCommand dependencyCommand;
        SqlDependency dependency;
        SqlDataReader queryReader;
        SqlCommand queryCommand;

        IHubContext<CarHub> carHubContext;

        const string queryString = "select Year, VIN, CarModels.Name as ModelName, CarFactories.Name as FactoryName from [dbo].CarProducts" + "\n" +
            "inner join [dbo].CarModels on CarModelId = [dbo].CarModels.Id" + "\n" +
            "inner join [dbo].CarFactories on FactoryId = [dbo].CarFactories.Id";

        public IndexModel(RabbitEntityConsumer.Models.CarsDBContext context, IHubContext<CarHub> hubContext)
        {
            _context = context;
            carHubContext = hubContext;

            EstablishConnection();
            CreateNewDependency();
        }

        public void EstablishConnection()
        {
            connection = new SqlConnection();
            connection.ConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Integrated Security=true;Database=CarsDB;";
            connection.Open();



            SqlDependency.Start("Data Source=(LocalDb)\\MSSQLLocalDB;Integrated Security=true;Database=CarsDB;");
        }

        public void CreateNewDependency()
        {
            dependencyCommand = new SqlCommand("SELECT Year FROM [dbo].CarProducts", connection);

            dependency = new SqlDependency(dependencyCommand);

            dependency.OnChange += OnDependencyChange;

            dependencyReader = dependencyCommand.ExecuteReader();
        }


        public IList<CarProducts> CarProducts { get;set; }

        public string DateTimeString { get; set; }


        public async Task OnGetAsync()
        {
            CarProducts = await _context.CarProducts
                .Include(c => c.CarModel)
                .Include(c => c.Factory).ToListAsync();
        }

        void OnDependencyChangeWEntity(object sender, SqlNotificationEventArgs e)
        {
            dependency.OnChange -= OnDependencyChangeWEntity;
            dependencyReader.Close();
            CreateNewDependency();

            var carProductsQuery = _context.CarProducts;

            carHubContext.Clients.All.SendAsync("DropTable");
            foreach (CarProducts product in carProductsQuery)
            {
                carHubContext.Clients.All.SendAsync("UpdateCars", product.Year, product.Vin, product.CarModel.Name, product.Factory.Name);
                //Debug.WriteLine(String.Format("{0}, {1}, {2}, {3}", queryReader["Year"], queryReader["VIN"], queryReader["ModelName"], queryReader["FactoryName"]));
            }
        }


        void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            queryCommand = new SqlCommand(queryString, connection);

            Debug.WriteLine(e.Info);
            dependency.OnChange -= OnDependencyChange;
            dependencyReader.Close();

            queryReader = queryCommand.ExecuteReader();

            carHubContext.Clients.All.SendAsync("DropTable");
            while (queryReader.Read())
            {                
                carHubContext.Clients.All.SendAsync("UpdateCars", queryReader["Year"], queryReader["VIN"], queryReader["ModelName"], queryReader["FactoryName"]);
                Debug.WriteLine(String.Format("{0}, {1}, {2}, {3}", queryReader["Year"], queryReader["VIN"], queryReader["ModelName"], queryReader["FactoryName"]));
            }
            queryReader.Close();

            CreateNewDependency();
        }
    }
}

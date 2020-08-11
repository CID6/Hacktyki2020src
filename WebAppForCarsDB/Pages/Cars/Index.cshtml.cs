using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RabbitEntityConsumer.Models;
using RabbitMQ.Client.Events;
using WebAppForCarsDB.Hubs;
using WebAppForCarsDB.Services;

namespace WebAppForCarsDB.Pages.Cars
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RabbitEntityConsumer.Models.CarsDBContext _context;
        private readonly IHubContext<CarHub> _carHubContext;
        private readonly ISqlDependencyManager _sqlDependencyManager;

        SqlDataReader dependencyReader;
        SqlConnection connection;
        SqlCommand dependencyCommand;
        SqlDependency dependency;
        SqlDataReader queryReader;
        SqlCommand queryCommand;


        const string queryString = "select Year, VIN, CarModels.Name as ModelName, CarFactories.Name as FactoryName from [dbo].CarProducts" + "\n" +
            "inner join [dbo].CarModels on CarModelId = [dbo].CarModels.Id" + "\n" +
            "inner join [dbo].CarFactories on FactoryId = [dbo].CarFactories.Id";

        public IList<CarProducts> CarProducts { get; set; }
        public string DateTimeString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Models { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CarModel { get; set; }


        public IndexModel(RabbitEntityConsumer.Models.CarsDBContext context, IHubContext<CarHub> hubContext, ISqlDependencyManager sqlDependencyManager)
        {
            _context = context;
            _carHubContext = hubContext;
            _sqlDependencyManager = sqlDependencyManager;

            SearchString = "";

            _sqlDependencyManager.SetAction(OnDependencyChangeInManager);

            //SqlDependency.Stop("Data Source=(LocalDb)\\MSSQLLocalDB;Integrated Security=true;Database=CarsDB;");
            //EstablishConnection();


            //if (dependency == null)
            //{
            //    CreateNewDependency(); 
            //}
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



        public async Task OnGetAsync()
        {
            await _sqlDependencyManager.WriteMessage("Message from OnGetAsync");

            var carProducts = from m in _context.CarProducts
                              select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                carProducts = carProducts.Where(s => s.CarModel.Name.Contains(SearchString));
            }

            CarProducts = await carProducts
                .Include(c => c.CarModel)
                .Include(c => c.Factory).ToListAsync();
        }

        //tbf
        void OnDependencyChangeWEntity(object sender, SqlNotificationEventArgs e)
        {
            dependency.OnChange -= OnDependencyChangeWEntity;
            dependencyReader.Close();
            CreateNewDependency();

            var carProductsQuery = _context.CarProducts;

            _carHubContext.Clients.All.SendAsync("DropTable");
            foreach (CarProducts product in carProductsQuery)
            {
                _carHubContext.Clients.All.SendAsync("UpdateCars", product.Year, product.Vin, product.CarModel.Name, product.Factory.Name);
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

            _carHubContext.Clients.All.SendAsync("DropTable");


            while (queryReader.Read())
            {
                //Debug.WriteLine((queryReader["ModelName"] as string));
                if ((queryReader["ModelName"] as string).Contains(SearchString))
                {
                    _carHubContext.Clients.All.SendAsync("UpdateCars", queryReader["Year"], queryReader["VIN"], queryReader["ModelName"], queryReader["FactoryName"]);
                    Debug.WriteLine(String.Format("{0}, {1}, {2}, {3}", queryReader["Year"], queryReader["VIN"], queryReader["ModelName"], queryReader["FactoryName"]));
                }
            }
            queryReader.Close();

            CreateNewDependency();
        }

        void OnDependencyChangeInManager(SqlDataReader queryReader)
        {
            _carHubContext.Clients.All.SendAsync("DropTable");

            Debug.WriteLine("SearchString = " + SearchString);
            while (queryReader.Read())
            {
                //Debug.WriteLine((queryReader["ModelName"] as string));
                if ((queryReader["ModelName"] as string).ToUpper().Contains(SearchString.ToUpper()))
                {
                    _carHubContext.Clients.All.SendAsync("UpdateCars", queryReader["Year"], queryReader["VIN"], queryReader["ModelName"], queryReader["FactoryName"]);
                    Debug.WriteLine(String.Format("{0}, {1}, {2}, {3}", queryReader["Year"], queryReader["VIN"], queryReader["ModelName"], queryReader["FactoryName"]));
                }
            }
            queryReader.Close();
        }
    }
}

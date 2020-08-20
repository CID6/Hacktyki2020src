using EFCarsDB.Data;
using EFCarsDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAppForCarsDB.Hubs;
using WebAppForCarsDB.Services;

namespace WebAppForCarsDB.Pages.Cars
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CarsDBContext _context;
        private readonly IHubContext<CarHub> _carHubContext;
        private readonly ISqlDependencyManager _sqlDependencyManager;
        private readonly CarHub _carHub;

        public IList<CarProducts> CarProducts { get; set; }
        public string DateTimeString { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool Astra { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Vectra { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Cruze { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Almera { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Epica { get; set; }
        public SelectList Models { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CarModel { get; set; }


        public IndexModel(CarsDBContext context, IHubContext<CarHub> hubContext, ISqlDependencyManager sqlDependencyManager, CarHub carHubDI)
        {
            _context = context;
            _carHubContext = hubContext;
            //all dependency based stuff is in dependency manager
            _sqlDependencyManager = sqlDependencyManager;
            _carHub = carHubDI;


            //only callback is added to the manager
            _sqlDependencyManager.SetAction(OnDependencyChangeInManager);
        }

        public async Task OnGetAsync()
        {

            var carProducts = from m in _context.CarProducts
                              select m;

            string astraString = (Astra) ? "Astra" : "";
            string vectraString = (Vectra) ? "Vectra" : "";
            string almeraString = (Almera) ? "Almera" : "";
            string cruzeString = (Cruze) ? "Cruze" : "";
            string epicaString = (Epica) ? "Epica" : "";

            CarProducts = await carProducts
                .Include(c => c.CarModel)
                .Include(c => c.Factory)
                .Where(c => c.CarModel.Name == astraString || c.CarModel.Name == vectraString || c.CarModel.Name == almeraString || c.CarModel.Name == cruzeString || c.CarModel.Name == epicaString)
                .ToListAsync();

            foreach (var car in CarProducts)
            {
                Debug.WriteLine(car.CarModel.Name);
            }

            //foreach(var car in CarProductsList)
            //{
            //    await _carHubContext.Clients.Group(car.CarModel.Name).SendAsync("UpdateCars", car.Year, car.Vin, car.CarModel.Name, car.Factory.Name);
            //    Debug.WriteLine(car.CarModel.Name);
            //}
        }



        void OnDependencyChangeInManager(SqlDataReader queryReader)
        {
            _carHubContext.Clients.All.SendAsync("DropTable");

            while (queryReader.Read())
            {
                _carHubContext.Clients.Group(queryReader["ModelName"].ToString()).SendAsync("UpdateCars", queryReader["Year"], queryReader["VIN"], queryReader["ModelName"], queryReader["FactoryName"]);
                //_carHubContext.Clients.All.SendAsync("UpdateCars", queryReader["Year"], queryReader["VIN"], queryReader["ModelName"], queryReader["FactoryName"]);
                Debug.WriteLine(String.Format("{0}, {1}, {2}, {3}", queryReader["Year"], queryReader["VIN"], queryReader["ModelName"], queryReader["FactoryName"]));
            }
            queryReader.Close();
        }

    }
}

using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppForCarsDB.Hubs
{
    public class CarHub : Hub
    {
        public async Task SendMessage(string year, string vin, string model, string factory)
        {
            await Clients.All.SendAsync("UpdateCars", year, vin, model, factory);
        }

    }
}

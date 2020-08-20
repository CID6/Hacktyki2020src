using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAppForCarsDB.Hubs
{
    public class CarHub : Hub
    {
        public async Task SendCar(string year, string vin, string model, string factory)
        {
            await Clients.All.SendAsync("UpdateCars", year, vin, model, factory);
        }

        public Task JoinGroup(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task LeaveGroup(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public Task SendCarToGroup(string groupName, string year, string vin, string model, string factory)
        {
            return Clients.Group(groupName).SendAsync("UpdateCars", year, vin, model, factory);
        }

    }
}

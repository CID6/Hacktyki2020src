using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EFCarsDB.Data;
using EFCarsDB.Models;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace WebAppForCarsDB.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly EFCarsDB.Data.WebAppForCarsDBContext _context;

        IFirebaseConfig config;
        IFirebaseClient client;

        public IndexModel(EFCarsDB.Data.WebAppForCarsDBContext context)
        {
            _context = context;


            config = new FirebaseConfig
            {
                AuthSecret = "Vsu4bOeEDQj2WVc8iuTQibm79n5kmzaXLAlLaDBr",
                BasePath = "https://fir-hacktyki.firebaseio.com/"
            };

            client = new FirebaseClient(config);
            SetupListener();
        }

        public IList<FirebaseMovie> Movie { get;set; }

        public async Task OnGetAsync()
        {
            //Movie = await _context.Movie.ToListAsync();
            Movie = new List<FirebaseMovie>();

            FirebaseResponse response = await client.GetAsync("/Movies");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            foreach(var item in data)
            {
                FirebaseMovie deserializedMovie = JsonConvert.DeserializeObject<FirebaseMovie>(((JProperty)item).Value.ToString());
                Movie.Add(deserializedMovie);
                Debug.WriteLine(deserializedMovie.FirebaseID);
            }
        }

        private async Task SetupListener()
        {
            EventStreamResponse response = await client.OnAsync("Movies/", (sender, args, context) =>
            {
                Debug.WriteLine("Event fired! + " + args.Data);
            });

            Debug.WriteLine("EventSetup!");
        }
    }
}

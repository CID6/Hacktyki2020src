using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EFCarsDB.Data;
using EFCarsDB.Models;
using FireSharp.Interfaces;
using FireSharp.Config;
using Google.Apis.Logging;
using System.Diagnostics;
using FireSharp;
using FireSharp.Response;

namespace WebAppForCarsDB.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly EFCarsDB.Data.WebAppForCarsDBContext _context;

        IFirebaseConfig config;
        IFirebaseClient client;

        public CreateModel(EFCarsDB.Data.WebAppForCarsDBContext context)
        {
            _context = context;

            config = new FirebaseConfig
            {
                AuthSecret = "Vsu4bOeEDQj2WVc8iuTQibm79n5kmzaXLAlLaDBr",
                BasePath = "https://fir-hacktyki.firebaseio.com/"
            };

            client = new FirebaseClient(config);
        }

        

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FirebaseMovie Movie { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            await AddMovieToFirebaseAsync(Movie);
            Debug.WriteLine("Added to firebase successfully");


            return RedirectToPage("./Index");
        }

        private async Task AddMovieToFirebaseAsync(FirebaseMovie movie)
        {
            PushResponse response = client.Push("Movies/", movie);
            Debug.WriteLine(response.Result.name);
            movie.FirebaseID = response.Result.name;
            await client.SetAsync("Movies/" + movie.FirebaseID, movie);
        }
    }
}

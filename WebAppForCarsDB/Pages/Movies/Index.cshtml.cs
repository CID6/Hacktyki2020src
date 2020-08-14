using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EFCarsDB.Data;
using EFCarsDB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace WebAppForCarsDB.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly EFCarsDB.Data.WebAppForCarsDBContext _context;


        public IndexModel(EFCarsDB.Data.WebAppForCarsDBContext context)
        {
            _context = context;


        }

        public IList<FirebaseMovie> Movie { get;set; }

        public async Task OnGetAsync()
        {
            //Movie = await _context.Movie.ToListAsync();
            Movie = new List<FirebaseMovie>();

        }

    }
}

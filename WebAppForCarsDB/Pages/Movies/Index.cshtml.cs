using EFCarsDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppForCarsDB.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly EFCarsDB.Data.WebAppForCarsDBContext _context;

        [BindProperty(SupportsGet = true)]
        public bool Action { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Comedy { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Documentary { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Scifi { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Horror { get; set; }


        public IndexModel(EFCarsDB.Data.WebAppForCarsDBContext context)
        {
            _context = context;


        }

        public IList<FirebaseMovie> Movie { get; set; }

        public async Task OnGetAsync()
        {
            Movie = new List<FirebaseMovie>();

        }

    }
}

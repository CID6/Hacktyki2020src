using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EFCarsDB.Data;
using EFCarsDB.Models;
using Google.Apis.Logging;
using System.Diagnostics;

namespace WebAppForCarsDB.Pages.Movies
{
    public class CreateModel : PageModel
    {
        //all firebase code is in movieCreate.js

        private readonly EFCarsDB.Data.WebAppForCarsDBContext _context;


        public CreateModel(EFCarsDB.Data.WebAppForCarsDBContext context)
        {
            _context = context;
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

            return RedirectToPage("./Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EFCarsDB.Models;
using EFCarsDB.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebAppForCarsDB.Pages.Cars
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly CarsDBContext _context;

        public CreateModel(CarsDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "Name");
        ViewData["FactoryId"] = new SelectList(_context.CarFactories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public CarProducts CarProducts { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CarProducts.Add(CarProducts);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

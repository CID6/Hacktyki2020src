using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RabbitEntityConsumer.Models;

namespace WebAppForCarsDB.Pages.Cars
{
    public class EditModel : PageModel
    {
        private readonly RabbitEntityConsumer.Models.CarsDBContext _context;

        public EditModel(RabbitEntityConsumer.Models.CarsDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CarProducts CarProducts { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarProducts = await _context.CarProducts
                .Include(c => c.CarModel)
                .Include(c => c.Factory).FirstOrDefaultAsync(m => m.Id == id);

            if (CarProducts == null)
            {
                return NotFound();
            }
           ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "Name");
           ViewData["FactoryId"] = new SelectList(_context.CarFactories, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CarProducts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarProductsExists(CarProducts.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CarProductsExists(int id)
        {
            return _context.CarProducts.Any(e => e.Id == id);
        }
    }
}

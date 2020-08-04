using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RabbitEntityConsumer.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebAppForCarsDB.Pages.Cars
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly RabbitEntityConsumer.Models.CarsDBContext _context;

        public DeleteModel(RabbitEntityConsumer.Models.CarsDBContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarProducts = await _context.CarProducts.FindAsync(id);

            if (CarProducts != null)
            {
                _context.CarProducts.Remove(CarProducts);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

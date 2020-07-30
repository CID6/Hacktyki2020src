using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RabbitEntityConsumer.Models;

namespace WebAppForCarsDB.Pages.Cars
{
    public class DetailsModel : PageModel
    {
        private readonly RabbitEntityConsumer.Models.CarsDBContext _context;

        public DetailsModel(RabbitEntityConsumer.Models.CarsDBContext context)
        {
            _context = context;
        }

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
    }
}

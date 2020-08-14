using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EFCarsDB.Models;
using EFCarsDB.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebAppForCarsDB.Pages.Cars
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly CarsDBContext _context;

        public DetailsModel(CarsDBContext context)
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

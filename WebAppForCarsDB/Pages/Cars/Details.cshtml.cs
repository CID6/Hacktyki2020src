using EFCarsDB.Data;
using EFCarsDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

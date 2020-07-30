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
    public class IndexModel : PageModel
    {
        private readonly RabbitEntityConsumer.Models.CarsDBContext _context;

        public IndexModel(RabbitEntityConsumer.Models.CarsDBContext context)
        {
            _context = context;
        }

        public IList<CarProducts> CarProducts { get;set; }

        public async Task OnGetAsync()
        {
            CarProducts = await _context.CarProducts
                .Include(c => c.CarModel)
                .Include(c => c.Factory).ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAppForCarsDB.Models;

namespace WebAppForCarsDB.Data
{
    public class WebAppForCarsDBContext : DbContext
    {
        public WebAppForCarsDBContext (DbContextOptions<WebAppForCarsDBContext> options)
            : base(options)
        {
        }

        public DbSet<WebAppForCarsDB.Models.Movie> Movie { get; set; }
    }
}

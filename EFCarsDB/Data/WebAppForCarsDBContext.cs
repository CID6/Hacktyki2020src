using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCarsDB.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCarsDB.Data
{
    public class WebAppForCarsDBContext : IdentityDbContext<WebAppForCarsDBUser>
    {
        public WebAppForCarsDBContext (DbContextOptions<WebAppForCarsDBContext> options)
            : base(options)
        {
        }

        public DbSet<EFCarsDB.Models.Movie> Movie { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

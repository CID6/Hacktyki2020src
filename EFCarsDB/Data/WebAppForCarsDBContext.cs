using EFCarsDB.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCarsDB.Data
{
    /// <summary>
    /// This is the context class for the MOVIE database
    /// </summary>
    /// Not currently in use, replaced with firebase class
    public class WebAppForCarsDBContext : IdentityDbContext<WebAppForCarsDBUser>
    {
        public WebAppForCarsDBContext(DbContextOptions<WebAppForCarsDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

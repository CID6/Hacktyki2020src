using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAppForCarsDB.Areas.Identity.Data;
using WebAppForCarsDB.Data;

[assembly: HostingStartup(typeof(WebAppForCarsDB.Areas.Identity.IdentityHostingStartup))]
namespace WebAppForCarsDB.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UserContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UserContextConnection")));

                services.AddDefaultIdentity<WebAppForCarsDBUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<UserContext>();
            });
        }
    }
}
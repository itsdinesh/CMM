using System;
using CMM.Areas.Identity.Data;
using CMM.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CMM.Areas.Identity.IdentityHostingStartup))]
namespace CMM.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<CMMContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("CMMContextConnection")));

                services.AddDefaultIdentity<CMMUser>(options => options.SignIn.RequireConfirmedAccount = true)
                 .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<CMMContext>();
            });
        }
    }
}
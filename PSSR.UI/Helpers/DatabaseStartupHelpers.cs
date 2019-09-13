using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.DatabaseServices.Concrete;

namespace PSSR.UI
{
    public static class DatabaseStartupHelpers
    {

        private static readonly string WwwRootDirectory = $"wwwroot{Path.DirectorySeparatorChar}";

        public static string GetWwwRootPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), WwwRootDirectory);
        }

        public static IWebHost SetupDevelopmentDatabase(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                using (var context = services.GetRequiredService<EfCoreContext>())
                {
                    try
                    {
                        context.DevelopmentEnsureCreated();
                        context.SeedDatabase(GetWwwRootPath());
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while setting upor seeding the development database.");
                    }
                }
            }

            return webHost;
        }
    }
}

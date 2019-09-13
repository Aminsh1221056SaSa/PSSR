using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PSSR.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            //see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging?tabs=aspnetcore2x#how-to-add-providers
            //var webHost = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .ConfigureAppConfiguration((hostingContext, config) =>
            //    {
            //        var env = hostingContext.HostingEnvironment;
            //        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            //        config.AddEnvironmentVariables();
            //    })
            //    //Normally we would configure logging here, but on production needed access to IHttpContextAccessor, so on production we had to do it in the Configure method
            //    .ConfigureLogging((hostingContext, logging) =>
            //    {
            //        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            //        logging.AddConsole();
            //        logging.AddDebug();
            //    })
            //    .UseStartup<Startup>()
            //    .Build()
            //    .SetupDevelopmentDatabase();

            //webHost.Run();


        }

        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .Build().SetupDevelopmentDatabase();
    }
}

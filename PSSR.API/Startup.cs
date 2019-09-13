using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PSSR.API.Helper;
using PSSR.DataLayer.EfCode;
using AutoMapper;
using System.Security.Claims;

namespace PSSR.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                     .SetBasePath(env.ContentRootPath)
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var section = Configuration.GetSection("AppSettings");
            var apSetting = section.Get<ApplicationSettings>();

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<EfCoreContext>(options => options.UseSqlServer(connection));

            services.AddMvc(options =>
            {
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication("Bearer")
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = apSetting.Authority;
                   options.RequireHttpsMetadata = false;
                   options.ApiName = apSetting.OilApiName;
               });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("dataEventRecordsSuperAdmin", policyAdmin =>
                {
                    policyAdmin.RequireClaim(ClaimTypes.Role, "SuperAdministrator");
                });

                options.AddPolicy("dataEventRecordsAdmin", policyAdmin =>
                {
                    policyAdmin.RequireClaim(ClaimTypes.Role, "SuperAdministrator", "Administrator");
                });

                options.AddPolicy("dataEventRecordsManager", policyAdmin =>
                {
                    policyAdmin.RequireClaim(ClaimTypes.Role, "SuperAdministrator", "Administrator", "Manager");
                });

                options.AddPolicy("dataEventRecordsCustomer", policyAdmin =>
                {
                    policyAdmin.RequireClaim(ClaimTypes.Role, "SuperAdministrator", "Administrator", "Manager", "Customer");
                });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGen(SwaggerHelper.ConfigureSwaggerGen);
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseSwagger(SwaggerHelper.ConfigureSwagger);
            app.UseSwaggerUI(SwaggerHelper.ConfigureSwaggerUI);
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}

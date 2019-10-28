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
using Autofac;
using BskaGenericCoreLib.Configuration;
using PSSR.API.App_Start;
using Autofac.Extensions.DependencyInjection;
using System;

namespace PSSR.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var section = Configuration.GetSection("AppSettings");
            var apSetting = section.Get<ApplicationSettings>();

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<EfCoreContext>(options => options.UseSqlServer(connection));

            services.AddMvc(options =>
            {
               options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddAuthentication("Bearer")
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = apSetting.Authority;
                   options.RequireHttpsMetadata = false;
                   options.ApiName = apSetting.OilApiName;
               });

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("https://localhost:44349")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
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
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AppMapping());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            var containerBuilder = new ContainerBuilder();

            #region GenericBizRunner parts
            // Need to call AddAutoMapper to set up the mappings any GenericAction From/To Biz Dtos
            //GenericBizRunner has two AutoFac modules that can register all the services needed
            //This one is the simplest, as it sets up the link to the application's DbContext
            containerBuilder.RegisterModule(new BskaGenericDiModule<EfCoreContext>());
            //Now I use the ServiceLayer AutoFac module that registers all the other DI items, such as my biz logic
            containerBuilder.RegisterModule(new UIModule());
            #endregion

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName== "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("default");
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

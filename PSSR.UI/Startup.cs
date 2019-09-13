using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using PSSR.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Autofac.Extensions.DependencyInjection;
using BskaGenericCoreLib.Configuration;
using Microsoft.Extensions.Logging;
using PSSR.UI.Helpers;
using PSSR.UI.Hubs;
using PSSR.UI.Configuration;
using System.IdentityModel.Tokens.Jwt;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.App_Start;
using PSSR.UI.Helpers.Security;
using Microsoft.AspNetCore.Authorization;
using PSSR.UI.Helpers.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using PSSR.UserSecurity.Configuration.IdentityContextModels;

namespace PSSR.UI
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            var section = Configuration.GetSection("AppSettings");
            var apSetting = section.Get<ApplicationSettings>();

            services.Configure<ApplicationSettings>(section);
            services.Configure<SqlConnectionHelper>(Configuration.GetSection("ConnectionStrings"));
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<EfCoreContext>(options => options.UseSqlServer(connection));

            services.AddDbContext<AppIdentityDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<AppUser, Role>()
            //    .AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddMvc(options =>
            {
            }).AddViewOptions(options =>
            {
                options.HtmlHelperOptions.ClientValidationEnabled = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.AddTransient<CookieEventHandler>();
            //services.AddSingleton<LogoutUserManager>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie(option=>{
                option.AccessDeniedPath = "/Home/Privacy";
                option.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                option.Cookie.Name = "POECCookie";

                //option.EventsType = typeof(CookieEventHandler);
            })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    options.Authority = apSetting.Authority;
                    options.RequireHttpsMetadata = false;

                    options.ClientId = apSetting.ClientId;
                    options.ClientSecret = "PCMS_WEB_APP_SECRET";
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("offline_access");
                    //add oil api scope
                    options.Scope.Add(apSetting.OilApiName);
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("dataEventRecordsSuperAdmin",
                  policy => policy
                  .AddRequirements(new PersonPolicyRequirment(true,new string[] { "SuperAdministrator" })));

                options.AddPolicy("dataEventRecordsAdmin",
                 policy => policy
                 .AddRequirements(new PersonPolicyRequirment(true, new string[] { "SuperAdministrator", "Administrator" })));

                options.AddPolicy("dataEventRecordsManager",
                policy => policy
               .AddRequirements(new PersonPolicyRequirment(true, new string[] { "SuperAdministrator", "Administrator","Manager" })));

                options.AddPolicy("dataEventRecordsCustomer",
                policy => policy
               .AddRequirements(new PersonPolicyRequirment(true, new string[] { "SuperAdministrator", "Administrator", "Manager", "Customer" })));
            });

            services.AddScoped<IAuthorizationHandler, SuperAdminPolicyHandle>();
            services.AddTransient<IHttpClient, StandardHttpClient>();

            services.AddAutoMapper();
            services.AddSwaggerGen(SwaggerHelper.ConfigureSwaggerGen);
            //signalR
            services.AddSignalR();
            services.AddSignalRCore();
            //--------------------------------------------------------------------

            //Now we use AutoFac to do some of the more complex registering of services
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
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor,
            INavigationCacheOperations navigationCacheOperations, IAdminNavigationHelper adminNavigationCashOperations)
        {
            //loggerFactory.AddProvider(new RequestTransientLogger(() => httpContextAccessor));
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
          
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<WBSRoadMapHub>("/WBSRoadMap");
            });

            app.UseSwagger(SwaggerHelper.ConfigureSwagger);
            app.UseSwaggerUI(SwaggerHelper.ConfigureSwaggerUI);

            app.UseMvc(routes =>
            {
                //routes.MapRoute(name: "areaRoute",
                //template: "{area:exists}/{controller=Home}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
           
            await navigationCacheOperations.CreateNavigationCacheAsync();
            await adminNavigationCashOperations.CreateNavigationCacheAsync();
        }
    }
}

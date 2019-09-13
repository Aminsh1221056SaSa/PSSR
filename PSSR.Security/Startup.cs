
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PSSR.Security.Configuration.IdentityServer;
using PSSR.Security.Helpers;
using PSSR.UserSecurity.Configuration.IdentityContextModels;
using PSSR.UserSecurity.Models;

namespace PSSR.Security
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ConfigurationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<SqlConnectionHelper>(Configuration.GetSection("ConnectionStrings"));

            services.AddIdentity<AppUser, Role>()
                .AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            services.AddMvc(options =>
            {
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
           .AddDeveloperSigningCredential()
           .AddInMemoryPersistedGrants()
           .AddConfigurationStore(options =>
           {
               options.ConfigureDbContext = builder =>
               builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
               sql => sql.MigrationsAssembly(migrationsAssembly));
           })
          // this adds the operational data from DB (codes, tokens, consents)
          .AddOperationalStore(options =>
          {
              options.ConfigureDbContext = builder =>
              builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
              sql => sql.MigrationsAssembly(migrationsAssembly));
              // this enables automatic token cleanup. this is optional.
              options.EnableTokenCleanup = true;
              options.TokenCleanupInterval = 30;
          }).AddAspNetIdentity<AppUser>()
            .AddProfileService<ProfileService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("dataEventRecordsAdmin",
                  policy => policy
                  .AddRequirements(new PersonPolicyRequirment(true)));
            });

            services.AddScoped<IAuthorizationHandler, SuperAdminPolicyHandle>();
            services.AddSingleton<IDatabaseService, DatabaseService>();
        }

        public IConfiguration Configuration { get; }

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

            //IdentityServerAuthenticationOptions identityServerValidationOptions = new IdentityServerAuthenticationOptions
            //{
            //    Authority = Config.HOST_URL + "/",
            //    AllowedScopes = new List<string> { "dataEventRecords" },
            //    ApiSecret = "dataEventRecordsSecret",
            //    ApiName = "dataEventRecords",
            //    AutomaticAuthenticate = true,
            //    SupportedTokens = SupportedTokens.Both,
            //    // TokenRetriever = _tokenRetriever,
            //    // required if you want to return a 403 and not a 401 for forbidden responses
            //     = true,
            //};

            app.UseCors("CorsPolicy");
            app.UseIdentityServer();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            //app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=Index}");
            });

            AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices).Wait();
            DatabaseStartupHelpers.InitializeIdentityServerDatabase(app);
        }
    }
}

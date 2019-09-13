
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PSSR.UserSecurity.Models;
using PSSR.UserSecurity.Models.EntityConfiguration;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PSSR.UserSecurity.Configuration.IdentityContextModels
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser,Role,string>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
             : base(options)
        { }

        public DbSet<NavigationMenuType> NavigationMenus { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NavigationMenuConfig());
            modelBuilder.ApplyConfiguration(new NavigationMenuRoleConfig());

            base.OnModelCreating(modelBuilder);
        }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider)
        {
            UserManager<AppUser> userManager =
                serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<Role> roleManager =
                serviceProvider.GetRequiredService<RoleManager<Role>>();

            string username = "APSE";
            string email = "sahranavard.amin@gmail.com";
            string password = "015266Sa@";
            string role = "SuperAdministrator";

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new Role(role));
                }

                AppUser user = new AppUser
                {
                    UserName = username,
                    Email = email
                };

                IdentityResult result = await userManager
                     .CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);

                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Name, user.UserName));
                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Email, user.Email));
                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Role, role));
                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Actor, "MainManager"));
                }
            }
        }
    }

}

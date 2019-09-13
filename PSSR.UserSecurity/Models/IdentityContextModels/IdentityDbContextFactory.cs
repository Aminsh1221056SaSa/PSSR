using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PSSR.UserSecurity.Configuration.IdentityContextModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.UserSecurity.Models.IdentityContextModels
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
    {
        public AppIdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-L4SPE0L;Initial Catalog=Petro.Apse;User ID=sa;Password=1221056@Am", b => b.MigrationsAssembly("PSSR.UserSecurity"));

            return new AppIdentityDbContext(optionsBuilder.Options);
        }
    }
}

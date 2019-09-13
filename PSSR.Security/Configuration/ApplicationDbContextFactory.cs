using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PSSR.Security.Data.Migrations.IdentityServer
{

    class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-L4SPE0L;Initial Catalog=Petro.Apse;User ID=sa;Password=1221056@Am", b => b.MigrationsAssembly("PSSR.Security"));

            return new PersistedGrantDbContext(optionsBuilder.Options, new OperationalStoreOptions());
        }
    }

    class ConfigurationDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-L4SPE0L;Initial Catalog=Petro.Apse;User ID=sa;Password=1221056@Am", b => b.MigrationsAssembly("PSSR.Security"));

            return new ConfigurationDbContext(optionsBuilder.Options, new ConfigurationStoreOptions());
        }
    }
}

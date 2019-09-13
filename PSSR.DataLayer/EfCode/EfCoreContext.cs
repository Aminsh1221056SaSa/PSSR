using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfCode.Configurations;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore.Design;
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfClasses;

namespace PSSR.DataLayer.EfCode
{
    public class EfCoreContext : DbContext
    {
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<ProjectSystem> ProjectSystems { get; set; }
        public DbSet<ProjectSubSystem> ProjectSubSystems { get; set; }
        public DbSet<Descipline> Desciplines { get; set; }
        public DbSet<ValueUnit> ValueUnits { get; set; }
        public DbSet<Activity> Activites { get; set; }
        public DbSet<PunchType> PunchTypes { get; set; }
        public DbSet<PunchCategory> PunchCategories { get; set; }
        public DbSet<Punch> Punchs { get; set; }
        public DbSet<FormDictionary> FormDictionaries { get; set; }
        public DbSet<WorkPackage> ProjectRoadMaps { get; set; }
        private DbSet<WorkPackageStep> WorkPackageSteps { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<ProjectWBS> ProjectWBS { get; set; }
     
        public DbSet<WorkPackageStep> WorkPackageStep { get; set; }
        public DbSet<ActivityDocument> ActivityDocuments { get; set; }

        public DbSet<MDRStatus> MDRStatus { get; set; }
        public DbSet<MDRDocument> MDRDocuments { get; set; }
        public DbSet<MDRDocumentComment> MDRDocumentComments { get; set; }
        public DbSet<MDRStatusHistory> MdrStatusHistories { get; set; }

        public EfCoreContext(
           DbContextOptions<EfCoreContext> options)
           : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PersonConfig());
            modelBuilder.ApplyConfiguration(new ContractorConfig());
            modelBuilder.ApplyConfiguration(new PersonProjectConfig());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectSystemConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectSubSystemConfiguration());
            modelBuilder.ApplyConfiguration(new DesciplineConfig());
            modelBuilder.ApplyConfiguration(new ValueUnitConfig());
           // modelBuilder.ApplyConfiguration(new ValueUnitDesciplineConfig());
            //modelBuilder.ApplyConfiguration(new PriorityConfig());
            modelBuilder.ApplyConfiguration(new ActivityConfig());
            modelBuilder.ApplyConfiguration(new PunchTypeConfig());
            modelBuilder.ApplyConfiguration(new PunchConfig());
            modelBuilder.ApplyConfiguration(new FormDictionaryConfig());
            modelBuilder.ApplyConfiguration(new WorkPackageConfig());
            modelBuilder.ApplyConfiguration(new LocationTypeConfig());
            modelBuilder.ApplyConfiguration(new ProjectWBSConfig());
            modelBuilder.ApplyConfiguration(new ActivityStatusHistoryConfig());
            modelBuilder.ApplyConfiguration(new MDRDocumentConfig());
            modelBuilder.ApplyConfiguration(new MDRDocumentCommentConfig());
            modelBuilder.ApplyConfiguration(new FormDictionaryDesciplineConfig());
            modelBuilder.ApplyConfiguration(new WorkPackagePunchTypeConfig());
            modelBuilder.ApplyConfiguration(new WorkPackageStepConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityDocumentConfig());
            modelBuilder.ApplyConfiguration(new MDRStatusConfig());
            modelBuilder.ApplyConfiguration(new MDRStatusHistoryConfig());
            modelBuilder.ApplyConfiguration(new PunchCategoryConfiguration());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State ==
            EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditTracker)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedDate").CurrentValue = DateTime.Now;
                        entry.Property("UpdatedDate").CurrentValue = DateTime.Now;
                    }
                    else
                    {
                        entry.Property("UpdatedDate").CurrentValue = DateTime.Now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State ==
            EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditTracker)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedDate").CurrentValue = DateTime.Now;
                        entry.Property("UpdatedDate").CurrentValue = DateTime.Now;
                    }
                    else
                    {
                        entry.Property("UpdatedDate").CurrentValue = DateTime.Now;
                    }
                }
            }

            return base.SaveChanges();
        }
    }

    class ApplicationDbContextFactory : IDesignTimeDbContextFactory<EfCoreContext>
    {
        public EfCoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfCoreContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-L4SPE0L;Initial Catalog=PSSR.APSE;User ID=sa;Password=1221056@Am", b => b.MigrationsAssembly("Refinery.Archives.DataLayer"));

            return new EfCoreContext(optionsBuilder.Options);
        }
    }
}

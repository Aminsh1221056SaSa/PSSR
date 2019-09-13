using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class WorkPackageConfig : IEntityTypeConfiguration<WorkPackage>
    {
        public void Configure(EntityTypeBuilder<WorkPackage> builder)
        {
            builder.HasKey(pr => pr.Id);

            builder.Property(pr => pr.Name).IsRequired();

            builder.ToTable("ProjectRoadMap", "Production");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Projects;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.ContractorId).IsRequired();
            builder.Property(p => p.StartDate);
            builder.Property(p => p.EndDate);
            builder.Property(p => p.Type).IsRequired().HasDefaultValue(ProjectType.GasPlatform);
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.UpdatedDate).IsRequired();

            builder.ToTable("Project", "OrganizationResources");

            builder.HasOne(p => p.Contractor).WithMany(p => p.Projects).HasForeignKey(p => p.ContractorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

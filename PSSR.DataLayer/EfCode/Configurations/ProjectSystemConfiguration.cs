using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class ProjectSystemConfiguration : IEntityTypeConfiguration<ProjectSystem>
    {
        public void Configure(EntityTypeBuilder<ProjectSystem> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(ps => ps.Code).IsRequired().HasMaxLength(50);
            builder.Property(ps => ps.ProjectId).IsRequired();
            builder.Property(ps => ps.Type).IsRequired();
            builder.Property(ps => ps.Description).HasMaxLength(500);
            
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.UpdatedDate).IsRequired();

            builder.ToTable("ProjectSystem", "OrganizationResources");

            builder.HasOne(ps => ps.Project).WithMany(p => p.ProjectSystems).HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => new { s.Code })
            .ForSqlServerIsClustered(false).IsUnique(true).HasName("IX_SystemCode_Unique");
        }
    }
}

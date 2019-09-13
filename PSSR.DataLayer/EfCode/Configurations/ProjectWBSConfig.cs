using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class ProjectWBSConfig : IEntityTypeConfiguration<ProjectWBS>
    {
        public void Configure(EntityTypeBuilder<ProjectWBS> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(b => b.ParentId);
            builder.Property(b => b.ProjectId).IsRequired();
            builder.Property(b => b.TargetId).IsRequired();
            builder.Property(b => b.Type).IsRequired();
            builder.Property(b => b.WBSCode).HasMaxLength(150).IsRequired();
            builder.Property(b => b.Name).HasMaxLength(150);
            builder.Property(b => b.WF).IsRequired();

            builder.ToTable("ProjectWBS", "OrganizationResources");

            builder.HasOne(s => s.Parent).WithMany(s => s.Childeren).HasForeignKey(s => s.ParentId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Project).WithMany(s => s.ProjectWBS)
                .HasForeignKey(s => s.ProjectId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => new { s.WBSCode, s.ProjectId})
           .ForSqlServerIsClustered(false).IsUnique(true).HasName("IX_ProjectWBSCode_Unique");
        }
    }
}

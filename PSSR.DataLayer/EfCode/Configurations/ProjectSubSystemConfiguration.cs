using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class ProjectSubSystemConfiguration : IEntityTypeConfiguration<ProjectSubSystem>
    {
        public void Configure(EntityTypeBuilder<ProjectSubSystem> builder)
        {
            builder.HasKey(psb => psb.Id);

            builder.Property(ps => ps.Code).IsRequired().HasMaxLength(50);
            builder.Property(psb => psb.Description).HasMaxLength(500);
            builder.Property(psb => psb.ProjectSystemId).IsRequired();
            builder.Property(psb => psb.PriorityNo).IsRequired();
            builder.Property(psb => psb.SubPriorityNo);
            
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.UpdatedDate).IsRequired();

            builder.ToTable("ProjectSubSystem", "OrganizationResources");

            builder.HasOne(psb => psb.ProjectSystem).WithMany(psb => psb.ProjectSubSystems).HasForeignKey(psb => psb.ProjectSystemId)
                .OnDelete(DeleteBehavior.Cascade);

             builder.HasIndex(s => new { s.Code })
            .ForSqlServerIsClustered(false).IsUnique(true).HasName("IX_SubSystemCode_Unique");

            //builder.HasOne(psb => psb.Priority).WithMany(psb => psb.ProjectSubSystems).HasForeignKey(psb => psb.PriorityId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

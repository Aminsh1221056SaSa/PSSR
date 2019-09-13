using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects.MDRS;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class MDRStatusConfig : IEntityTypeConfiguration<MDRStatus>
    {
        public void Configure(EntityTypeBuilder<MDRStatus> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name).HasMaxLength(500);
            builder.Property(s => s.Wf).IsRequired();
            builder.Property(s => s.ProjectId).IsRequired();

            builder.ToTable("MDRStatus", "OrganizationResources");

            builder.HasOne(ps => ps.Project).WithMany(p => p.MDRStatus).HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

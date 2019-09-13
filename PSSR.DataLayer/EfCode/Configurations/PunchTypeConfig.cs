using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class PunchTypeConfig : IEntityTypeConfiguration<PunchType>
    {
        public void Configure(EntityTypeBuilder<PunchType> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);

            builder.ToTable("PunchType", "OrganizationResources");

            builder.HasOne(s => s.Project).WithMany(s => s.PunchTypes)
                .HasForeignKey(s => s.ProjectId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}

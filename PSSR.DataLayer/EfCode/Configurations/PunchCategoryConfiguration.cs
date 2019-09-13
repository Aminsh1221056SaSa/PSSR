using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class PunchCategoryConfiguration : IEntityTypeConfiguration<PunchCategory>
    {
        public void Configure(EntityTypeBuilder<PunchCategory> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);

            builder.ToTable("PunchCategory", "OrganizationResources");

            builder.HasOne(s => s.Project).WithMany(s => s.PunchCategoryes)
                .HasForeignKey(s => s.ProjectId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class WorkPackageStepConfiguration : IEntityTypeConfiguration<WorkPackageStep>
    {
        public void Configure(EntityTypeBuilder<WorkPackageStep> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Title).IsRequired().HasMaxLength(250);
            builder.Property(s => s.Description).HasMaxLength(500);
            builder.Property(s => s.WorkPackageId).IsRequired();

            builder.ToTable("WorkPackageStep", "Production");

            builder.HasOne(b => b.WorkPackage).WithMany(b => b.Steps)
             .HasForeignKey(f => f.WorkPackageId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class WorkPackagePunchTypeConfig : IEntityTypeConfiguration<WorkPackagePunchType>
    {
        public void Configure(EntityTypeBuilder<WorkPackagePunchType> builder)
        {
            builder.HasKey(p =>
              new { p.PunchTypeId, p.WorkPackageId }); //#A

            builder.Property(s => s.Precentage).IsRequired();
            //-----------------------------
            //Relationships

            builder.HasOne(pt => pt.PunchType)        //#C
                .WithMany(p => p.WorkPackages)   //#C
                .HasForeignKey(pt => pt.PunchTypeId);//#C

            builder.HasOne(pt => pt.WorkPackage)        //#C
                .WithMany(t => t.PunchTypes)       //#C
                .HasForeignKey(pt => pt.WorkPackageId);//#C

            builder.ToTable("WorkPackagePunchType", "Production");
        }
    }
}

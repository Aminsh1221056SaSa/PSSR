using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class ActivityConfig : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(ac => ac.Id);

            builder.Property(ac => ac.ActivityCode).IsRequired().HasMaxLength(150);
            builder.Property(ac => ac.ActualEndDate);
            builder.Property(ac => ac.ActualMh).IsRequired();
            builder.Property(ac => ac.ActualStartDate);
            builder.Property(ac => ac.EstimateMh).IsRequired();
            builder.Property(ac => ac.PlanEndDate);
            builder.Property(ac => ac.PlanStartDate);
            builder.Property(ac => ac.Progress).IsRequired();
            builder.Property(ac => ac.Status).IsRequired();
            builder.Property(ac => ac.TagDescription).HasMaxLength(500);
            builder.Property(ac => ac.TagNumber).IsRequired().HasMaxLength(50);
            builder.Property(ac => ac.ValueUnitNum).IsRequired();
            builder.Property(ac => ac.WeightFactor).IsRequired();
            builder.Property(s => s.CreatedDate).IsRequired();
            builder.Property(s => s.UpdatedDate).IsRequired();

            builder.Property(ac => ac.ValueUnitId).IsRequired();
            builder.Property(ac => ac.LocationId).IsRequired();
            builder.Property(ac => ac.WorkPackageId).IsRequired();

            builder.Property(ac => ac.WorkPackageStepId).IsRequired();
            builder.Property(ac => ac.SubsytemId).IsRequired();
            builder.Property(ac => ac.DesciplineId).IsRequired();
            builder.Property(ac => ac.FormDictionaryId).IsRequired();

            builder.ToTable("Activity", "OrganizationResources");

            builder.HasOne(pt => pt.ValueUnit)        //#C
             .WithMany(t => t.Activityes)       //#C
             .HasForeignKey(pt => pt.ValueUnitId).OnDelete(DeleteBehavior.Restrict);//#C

            builder.HasOne(pt => pt.FormDictionary)        //#C
            .WithMany(t => t.Activityes)       //#C
            .HasForeignKey(pt => pt.FormDictionaryId).OnDelete(DeleteBehavior.Restrict);//#C

            builder.HasOne(s => s.WorkPackage).WithMany(s => s.Activityes)
             .HasForeignKey(s => s.WorkPackageId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Location).WithMany(s => s.Activityes)
            .HasForeignKey(s => s.LocationId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.SubSystem).WithMany(s => s.Activityes)
            .HasForeignKey(s => s.SubsytemId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Descipline).WithMany(s => s.Activitys)
            .HasForeignKey(s => s.DesciplineId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s=>s.WorkPackageStep).WithMany(s=>s.Activities)
            .HasForeignKey(s => s.WorkPackageStepId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}

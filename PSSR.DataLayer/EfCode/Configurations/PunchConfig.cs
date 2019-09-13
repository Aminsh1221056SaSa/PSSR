using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class PunchConfig : IEntityTypeConfiguration<Punch>
    {
        public void Configure(EntityTypeBuilder<Punch> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ActivityId).IsRequired();
            builder.Property(p => p.ApproveBy).HasMaxLength(150);
            builder.Property(p => p.CheckBy).HasMaxLength(150);
            builder.Property(p => p.CheckDate);
            builder.Property(p => p.ClearDate);
            builder.Property(p => p.ClearPlan);
            builder.Property(p => p.ActualMh);
            builder.Property(p => p.EstimateMh);
            builder.Property(p => p.Code).IsRequired().HasMaxLength(50);
            builder.Property(p => p.CorectiveAction).HasMaxLength(500);
            builder.Property(p => p.ClearBy).HasMaxLength(150);
            builder.Property(p => p.DefectDescription).HasMaxLength(150);
            builder.Property(p => p.EnginerigRequired).IsRequired();
            builder.Property(p => p.MaterialRequired).IsRequired();
            builder.Property(p => p.OrginatedBy).HasMaxLength(150).IsRequired();
            builder.Property(p => p.OrginatedDate).IsRequired();
            builder.Property(p => p.PunchTypeId).IsRequired();
            builder.Property(p => p.VendorName).HasMaxLength(150);
            builder.Property(p => p.VendorRequired).IsRequired();

            builder.ToTable("Punch", "OrganizationResources");

            builder.HasOne(psb => psb.Activity).WithMany(psb => psb.Punchs).HasForeignKey(psb => psb.ActivityId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(psb => psb.PunchType).WithMany(psb => psb.Punches).HasForeignKey(psb => psb.PunchTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Category).WithMany(s => s.Punches)
               .HasForeignKey(s => s.CategoryId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}

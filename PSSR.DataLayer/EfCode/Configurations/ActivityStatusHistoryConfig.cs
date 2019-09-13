using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class ActivityStatusHistoryConfig : IEntityTypeConfiguration<ActivityStatusHistory>
    {
        public void Configure(EntityTypeBuilder<ActivityStatusHistory> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Description).HasMaxLength(500);
            builder.Property(s => s.ActivityId).IsRequired();
            builder.Property(s => s.CreateDate).IsRequired();
            builder.Property(s => s.HoldBy).IsRequired();

            builder.ToTable("ActivityStatusHistory", "OrganizationResources");

            builder.HasOne(pt => pt.Activity) 
             .WithMany(t => t.StatusHistory)
             .HasForeignKey(pt => pt.ActivityId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}

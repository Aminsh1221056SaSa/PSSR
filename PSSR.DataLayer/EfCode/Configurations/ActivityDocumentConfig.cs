using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class ActivityDocumentConfig : IEntityTypeConfiguration<ActivityDocument>
    {
        public void Configure(EntityTypeBuilder<ActivityDocument> builder)
        {
            builder.HasKey(ac => ac.Id);

            builder.Property(s => s.FilePath).IsRequired().HasMaxLength(500);
            builder.Property(s => s.Description).HasMaxLength(500);
            builder.Property(s => s.ActivityId).IsRequired();
            builder.Property(s => s.PunchId);
            builder.Property(s => s.CreatedDate).IsRequired();
            builder.Property(s => s.UpdatedDate).IsRequired();

            builder.ToTable("ActivityDocument", "OrganizationResources");

            builder.HasOne(s => s.Activity).WithMany(s => s.ActivityDocuments)
                .HasForeignKey(s => s.ActivityId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Punch).WithMany(s => s.ActivityDocuments)
                .HasForeignKey(s => s.PunchId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class MDRDocumentCommentConfig : IEntityTypeConfiguration<MDRDocumentComment>
    {
        public void Configure(EntityTypeBuilder<MDRDocumentComment> builder)
        {
            builder.HasKey(ac => ac.Id);

            builder.Property(s => s.Description).HasMaxLength(500);
            builder.Property(s => s.FilePath).HasMaxLength(500);
            builder.Property(s => s.CreatedDate).IsRequired();
            builder.Property(s => s.UpdatedDate).IsRequired();
            builder.Property(s => s.Title).IsRequired().HasMaxLength(200);
            builder.Property(s => s.MDRDocumentId).IsRequired();
            builder.Property(s => s.IsClear).IsRequired().HasDefaultValue(false);

            builder.ToTable("MDRDocumentComment", "OrganizationResources");

            builder.HasOne(pt => pt.MDRDocument)        //#C
             .WithMany(t => t.MDRDocumentComments)       //#C
             .HasForeignKey(pt => pt.MDRDocumentId).OnDelete(DeleteBehavior.Cascade); ;//#C
        }
    }
}

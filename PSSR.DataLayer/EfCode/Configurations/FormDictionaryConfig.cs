using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class FormDictionaryConfig : IEntityTypeConfiguration<FormDictionary>
    {
        public void Configure(EntityTypeBuilder<FormDictionary> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Code).IsRequired().HasMaxLength(50);
            builder.Property(f => f.CreatedDate).IsRequired();
            builder.Property(f => f.ActivityName).HasMaxLength(50);
            builder.Property(f => f.Description).HasMaxLength(500);
            builder.Property(f => f.WorkPackageId).IsRequired();
            builder.Property(f => f.UpdatedDate).IsRequired();
            builder.Property(s => s.FileName).HasMaxLength(500);
            builder.Property(s => s.Priority).IsRequired().HasDefaultValue(1);
            builder.Property(s => s.ManHours).IsRequired().HasDefaultValue(1);

            builder.ToTable("FormDictionary", "Production");

            builder.HasOne(b => b.WorkPackage).WithMany(b => b.FormDictionarys)
               .HasForeignKey(f => f.WorkPackageId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => new { s.Code})
           .ForSqlServerIsClustered(false).IsUnique(true).HasName("IX_FormCode_Unique");
        }
    }
}

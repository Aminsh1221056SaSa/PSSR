using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class DesciplineConfig : IEntityTypeConfiguration<Descipline>
    {
        public void Configure(EntityTypeBuilder<Descipline> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Description).HasMaxLength(500);
            builder.Property(d => d.CreatedDate).IsRequired();
            builder.Property(d => d.Name).IsRequired().HasMaxLength(50);
            builder.Property(d => d.UpdatedDate).IsRequired();

            builder.ToTable("Descipline", "Production");
        }
    }
}

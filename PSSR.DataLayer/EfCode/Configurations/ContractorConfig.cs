using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Person;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class ContractorConfig : IEntityTypeConfiguration<Contractor>
    {
        public void Configure(EntityTypeBuilder<Contractor> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(150);
            builder.Property(c => c.PhoneNumber).HasMaxLength(20);
            builder.Property(c => c.Address).HasMaxLength(500);
            builder.Property(c => c.ContractDate);
            builder.Property(c => c.CreatedDate).IsRequired();
            builder.Property(c => c.UpdatedDate).IsRequired();

            builder.ToTable("Contractor", "OrganizationResources");
        }
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Person;

namespace PSSR.DataLayer.EfCode.Configurations
{
    internal class PersonConfig : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(150);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(250);
            builder.Property(p => p.NationalId).IsRequired().HasMaxLength(10);
            builder.Property(p => p.MobileNumber).HasMaxLength(20);

            builder.ToTable("Person", "Person");

            builder.HasIndex(s => new { s.NationalId })
           .ForSqlServerIsClustered(false).IsUnique(true).HasName("IX_NationalId_Unique");
        }
    }
}

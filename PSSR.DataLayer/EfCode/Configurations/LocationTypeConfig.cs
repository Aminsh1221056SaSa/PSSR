using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class LocationTypeConfig : IEntityTypeConfiguration<LocationType>
    {
        public void Configure(EntityTypeBuilder<LocationType> builder)
        {
            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.Title).IsRequired().HasMaxLength(50);

            builder.ToTable("LocationType", "Production");
        }
    }
}

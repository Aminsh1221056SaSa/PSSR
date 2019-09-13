using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class ValueUnitConfig : IEntityTypeConfiguration<ValueUnit>
    {
        public void Configure(EntityTypeBuilder<ValueUnit> builder)
        {
            builder.HasKey(vl => vl.Id);

            builder.Property(vl => vl.MathNum).IsRequired();
            builder.Property(vl => vl.MathType).IsRequired();
            builder.Property(vl => vl.Name).IsRequired();
            builder.Property(vl => vl.ParentId);

            builder.ToTable("ValueUnit", "Production");

            builder.HasOne(s => s.Parent).WithMany(s => s.Childeren).HasForeignKey(s => s.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

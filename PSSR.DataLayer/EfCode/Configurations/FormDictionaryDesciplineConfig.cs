using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class FormDictionaryDesciplineConfig : IEntityTypeConfiguration<FormDictionaryDescipline>
    {
        public void Configure(EntityTypeBuilder<FormDictionaryDescipline> builder)
        {
            builder.HasKey(p =>
                new { p.FormDictionaryId, p.DesciplineId }); //#A

            //-----------------------------
            //Relationships

            builder.HasOne(pt => pt.Descipline)        //#C
                .WithMany(p => p.FormDictionaryLink)   //#C
                .HasForeignKey(pt => pt.DesciplineId);//#C

            builder.HasOne(pt => pt.FormDictionary)        //#C
                .WithMany(t => t.DesciplineLink)       //#C
                .HasForeignKey(pt => pt.FormDictionaryId);//#C

            builder.ToTable("FormDictionaryDescipline", "Production");
        }
    }
}

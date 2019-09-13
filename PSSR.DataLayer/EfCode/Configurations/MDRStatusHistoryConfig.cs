using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects.MDRS;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class MDRStatusHistoryConfig : IEntityTypeConfiguration<MDRStatusHistory>
    {
        public void Configure(EntityTypeBuilder<MDRStatusHistory> builder)
        {
            builder.HasKey(ac => ac.Id);

            builder.Property(s => s.Description).HasMaxLength(500);
            builder.Property(s => s.CreatedDate).IsRequired();
            builder.Property(s => s.UpdatedDate).IsRequired();
            builder.Property(s => s.MDRDocumentId).IsRequired();
            builder.Property(s => s.MdrStatusId).IsRequired();
            builder.Property(s => s.IsContractorConfirm).IsRequired();
            builder.Property(s => s.IsIFR).IsRequired();
            builder.Property(s => s.FolderName).IsRequired().HasMaxLength(50);

            builder.ToTable("MDRStatusHistory", "OrganizationResources");

            builder.HasOne(pt => pt.MDRDocument)        //#C
             .WithMany(t => t.MDRStatusHistoryies)       //#C
             .HasForeignKey(pt => pt.MDRDocumentId).OnDelete(DeleteBehavior.Cascade); ;//#C

            builder.HasOne(pt => pt.HStatus)        //#C
            .WithMany(t => t.MDRHistoryies)       //#C
            .HasForeignKey(pt => pt.MdrStatusId).OnDelete(DeleteBehavior.Restrict); ;//#C
        }
    }
}

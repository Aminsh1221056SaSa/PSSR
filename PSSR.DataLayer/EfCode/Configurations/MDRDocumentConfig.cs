using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Projects.MDRS;

namespace PSSR.DataLayer.EfCode.Configurations
{
    public class MDRDocumentConfig : IEntityTypeConfiguration<MDRDocument>
    {
        public void Configure(EntityTypeBuilder<MDRDocument> builder)
        {
            builder.HasKey(ac => ac.Id);

            builder.Property(s => s.Description).HasMaxLength(500);
            builder.Property(s => s.WorkPackageId).IsRequired();
            builder.Property(s => s.Title).IsRequired().HasMaxLength(200);
            builder.Property(s => s.CreatedDate).IsRequired();
            builder.Property(s => s.UpdatedDate).IsRequired();
            builder.Property(s => s.Code).IsRequired().HasMaxLength(250);
            builder.Property(s => s.Type).IsRequired().HasDefaultValue(Common.MDRDocumentType.A);
           

            builder.ToTable("MDRDocument", "OrganizationResources");

            builder.HasOne(pt => pt.Project)
             .WithMany(t => t.MDRDocuments)
             .HasForeignKey(pt => pt.ProjectId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pt => pt.WorkPackage)    
             .WithMany(t => t.MDRDocuments)       
             .HasForeignKey(pt => pt.WorkPackageId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => new { s.Code, s.ProjectId })
           .ForSqlServerIsClustered(false).IsUnique(true).HasName("IX_ProjectMDRCode_Unique");
        }
    }
}

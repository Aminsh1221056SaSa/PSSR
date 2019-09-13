using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.DataLayer.EfClasses.Person;

namespace PSSR.DataLayer.EfCode.Configurations
{
    class PersonProjectConfig : IEntityTypeConfiguration<PersonProject>
    {
        public void Configure(EntityTypeBuilder<PersonProject> builder)
        {
            builder.HasKey(p =>
                new { p.PersonId, p.ProjectId }); //#A

            //-----------------------------
            //Relationships

            builder.HasOne(pt => pt.Person)        //#C
                .WithMany(p => p.ProjectLink)   //#C
                .HasForeignKey(pt => pt.PersonId);//#C

            builder.HasOne(pt => pt.Project)        //#C
                .WithMany(t => t.AgentsLink)       //#C
                .HasForeignKey(pt => pt.ProjectId);//#C

            builder.ToTable("PersonProject", "Person");
        }
    }
}

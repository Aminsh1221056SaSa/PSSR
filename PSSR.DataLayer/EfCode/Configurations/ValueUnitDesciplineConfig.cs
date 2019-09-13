using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PSSR.DataLayer.EfCode.Configurations
{
    //public class ValueUnitDesciplineConfig : IEntityTypeConfiguration<ValueUnitDescipline>
    //{
    //    public void Configure(EntityTypeBuilder<ValueUnitDescipline> builder)
    //    {
    //        builder.HasKey(p =>
    //             new { p.ValueUnitId, p.DesciplineId }); //#A

    //        //-----------------------------
    //        //Relationships

    //        builder.HasOne(pt => pt.Descipline)        //#C
    //            .WithMany(p => p.ValueUnitsLink)   //#C
    //            .HasForeignKey(pt => pt.DesciplineId);//#C

    //        builder.HasOne(pt => pt.ValueUnit)        //#C
    //            .WithMany(t => t.DesciplineLink)       //#C
    //            .HasForeignKey(pt => pt.ValueUnitId);//#C

    //        builder.ToTable("ValueUnitDescipline", "Production");
    //    }
    //}
}

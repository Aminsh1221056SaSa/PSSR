using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSSR.UserSecurity.Models.IdentityContextModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.UserSecurity.Models.EntityConfiguration
{
    public sealed class NavigationMenuRoleConfig : IEntityTypeConfiguration<NavigationMenuItemRole>
    {
        public void Configure(EntityTypeBuilder<NavigationMenuItemRole> builder)
        {
            builder.HasKey(p =>
               new { p.NavigationMenuItemId, p.RoleId });

            builder.ToTable("NavigationMenuItemRole", "Setting");

            builder.HasOne(pt => pt.Role)        //#C
                .WithMany(p => p.NavigationItems)   //#C
                .HasForeignKey(pt => pt.RoleId).OnDelete(DeleteBehavior.Restrict);//#C
        }
    }
}

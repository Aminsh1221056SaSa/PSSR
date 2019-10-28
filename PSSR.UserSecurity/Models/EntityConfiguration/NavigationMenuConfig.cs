using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.UserSecurity.Models.EntityConfiguration
{
    public sealed class NavigationMenuConfig : IEntityTypeConfiguration<NavigationMenuType>
    {
        public void Configure(EntityTypeBuilder<NavigationMenuType> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.DisplayName).HasMaxLength(150).IsRequired();
            builder.Property(s => s.IsNested).IsRequired();
            builder.Property(s => s.Link).HasMaxLength(500);
            builder.Property(s => s.MaterialIcon).HasMaxLength(150);
            builder.Property(s => s.Sequence).IsRequired();
            builder.Property(s => s.ClientName).IsRequired();
            builder.Property(s => s.ParentId);
            builder.Property(s => s.Type).IsRequired();
            builder.ToTable("NavigationMenuType", "Setting");
            builder.HasMany(s => s.Roles)
                .WithOne(s => s.NavigationMenu).HasForeignKey(s => s.NavigationMenuItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Parent).WithMany(s => s.Childeren).HasForeignKey(s => s.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

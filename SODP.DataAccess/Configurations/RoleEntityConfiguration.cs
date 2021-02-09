﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;

namespace SODP.DataAccess.Configurations
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // A concurrency token for use with the optimistic concurrency checking
            builder.Property(r => r.ConcurrencyStamp)
                .IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            builder.Property(u => u.Name)
                .HasMaxLength(256);
            builder.Property(u => u.NormalizedName)
                .HasMaxLength(256);           
            
            // Primary key
            builder.HasKey(r => r.Id);

            // Index for "normalized" role name to allow efficient lookups
            builder.HasIndex(r => r.NormalizedName)
                .HasName("RoleNameIndex")
                .IsUnique();

            builder.ToTable("Roles");
        }
    }
}

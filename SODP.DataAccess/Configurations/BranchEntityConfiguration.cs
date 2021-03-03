﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.DataAccess.Configurations
{
    public class BranchEntityConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.Property(x => x.Sign)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.ToTable("Branches");

            builder.HasMany(x => x.Licences)
                .WithOne(x => x.Branch)
                .HasForeignKey(x => x.BranchId)
                .HasConstraintName("FK_Branch_Licence")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.DataAccess.Configurations
{
    public class LicenceEntityConfiguration : IEntityTypeConfiguration<Licence>
    {
        public void Configure(EntityTypeBuilder<Licence> builder)
        {
            builder.HasIndex(x => x.DesignerId)
                .HasName("IX_Designer");

            builder.ToTable("Licences");

            builder.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .HasConstraintName("FK_Branch")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

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
            builder.Property(x => x.Contents)
                .IsRequired();

            builder.HasIndex(x => x.DesignerId)
                .HasName("IX_Designer");

            builder.HasIndex(x => x.BranchId)
                .HasName("IX_Branch");

            builder.ToTable("Licences");



        }
    }
}

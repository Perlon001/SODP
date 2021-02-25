using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.DataAccess.Configurations
{
    public class DesignerEntityConfiguration : IEntityTypeConfiguration<Designer>
    {
        public void Configure(EntityTypeBuilder<Designer> builder)
        {
            builder.ToTable("Designers");

            builder.HasMany(x => x.Certificates)
                .WithOne(x => x.Designer)
                .HasForeignKey(x => x.DesignerId)
                .HasConstraintName("FK_Designer")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Licences)
                .WithOne(x => x.Designer)
                .HasForeignKey(x => x.DesignerId)
                .HasConstraintName("FK_Designer")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

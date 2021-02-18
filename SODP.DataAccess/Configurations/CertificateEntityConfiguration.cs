using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.DataAccess.Configurations
{
    public class CertificateEntityConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.ToTable("Certificates");
        }
    }
}

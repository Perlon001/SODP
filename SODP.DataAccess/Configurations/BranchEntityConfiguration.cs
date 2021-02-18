using Microsoft.EntityFrameworkCore;
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

            builder.ToTable("Branches");
        }
    }
}

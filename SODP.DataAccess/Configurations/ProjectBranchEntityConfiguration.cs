using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.DataAccess.Configurations
{
    public class ProjectBranchEntityConfiguration : IEntityTypeConfiguration<ProjectBranch>
    {
        public void Configure(EntityTypeBuilder<ProjectBranch> builder)
        {
            builder.ToTable("ProjectBranches");
        }
    }
}

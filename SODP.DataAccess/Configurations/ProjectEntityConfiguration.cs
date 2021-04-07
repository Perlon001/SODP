using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;

namespace SODP.DataAccess.Configurations
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Ignore(p => p.Symbol);

            builder.Property(p => p.Number)
                .HasColumnType("varchar(4)")
                .IsRequired();

            builder.Property(p => p.Title)
                .HasColumnType("nvarchar(250)")
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnType("longtext");

            builder.Property(p => p.Location)
                .HasColumnType("nvarchar(250)");

            builder.HasIndex(p => new { p.Number, p.StageId })
                .IsUnique()
                .HasName("IX_NumberStage");

            builder.HasIndex(p => p.StageId)
                .HasName("IX_Stage");

            builder.ToTable("Projects");

            //builder.HasOne(x => x.stage)
            //    .withmany()
            //    .hasforeignkey(x => x.stageid)
            //    .hasconstraintname("fk_project_stage")
            //    .ondelete(deletebehavior.restrict);

            //builder.HasMany(x => x.Branches)
            //    .WithOne(x => x.Project)
            //    .HasForeignKey(x => x.ProjectId)
            //    .HasConstraintName("FK_Project")
            //    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

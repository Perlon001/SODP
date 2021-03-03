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
                .HasColumnName("Number")
                .HasColumnType("Char(4)")
                .IsRequired();

            builder.Property(p => p.Title)
                .HasColumnName("Title")
                .IsRequired();
            
            builder.HasIndex(p => new { p.Number, p.StageId })
                .IsUnique()
                .HasName("IX_NumberStage");

            builder.HasIndex(p => p.StageId)
                .HasName("IX_Stage");

            builder.ToTable("Projects");

            builder.HasOne(x => x.Stage)
                .WithMany()
                .HasForeignKey(x => x.StageId)
                .HasConstraintName("FK_Project_Stage")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Branches)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .HasConstraintName("FK_Project")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

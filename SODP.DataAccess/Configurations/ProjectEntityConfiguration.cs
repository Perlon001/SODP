using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;

namespace SODP.DataAccess.Configurations
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            //builder.Property(p => p.Id)
            //    .HasColumnName("ID")
            //    .HasColumnType("Int")
            //    .UseMySqlIdentityColumn()
            //    .IsRequired();

            //builder.Property(p => p.Number)
            //    .IsRequired()
            //    .HasColumnName("PR_NUMBER")
            //    .HasColumnType("Char(4)");

            //builder.Property(p => p.StageSign)
            //    .IsRequired()
            //    .HasColumnName("PR_STAGESIGN")
            //    .HasColumnType("Char(10)");

            //builder.Property(p => p.Title)
            //    .HasColumnName("PR_TITLE")
            //    .HasColumnType("VarChar(100)")
            //    .IsRequired();

            //builder.Property(p => p.Description)
            //    .HasColumnName("PR_DESCRIPTION")
            //    .HasColumnType("Text");

            //builder.Property(p => p.Location)
            //    .HasColumnName("PR_LOCATION")
            //    .HasColumnType("VarChar(1000)");

            //builder.HasKey(s => s.Id)
            //    .HasName("IX_KEY");

            //builder.HasIndex(s => s.StageSign)
            //    .HasName("IX_StageSign");

            builder.Ignore(p => p.Symbol);
            
            builder.HasIndex(p => new { p.Number, p.StageId })
                .IsUnique()
                .HasName("IX_NumberStage");

            builder.HasIndex(p => p.StageId)
                .HasName("IX_Stage");

            builder.ToTable("Projects");

            builder.HasOne("SODP.Model.Stage", "Stage")
                .WithMany()
                .HasForeignKey("StageId")
                .HasConstraintName("FK_Stage")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

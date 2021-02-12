using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;

namespace SODP.DataAccess.Configurations
{
    public class StageEntityConfiguration : IEntityTypeConfiguration<Stage>
    {
        public void Configure(EntityTypeBuilder<Stage> builder)
        {
            //builder.Property(p => p.Sign)
            //    .HasColumnName("ST_SIGN")
            //    .HasColumnType("Char(10)");

            //builder.Property(p => p.Description)
            //    .IsRequired()
            //    .HasColumnName("ST_DESCRIPTION")
            //    .HasColumnType("Text")
            //    .HasDefaultValue("");

            //builder.HasKey(s => s.Id)
            //    .HasName("PK_Stages");

            builder.Property(p => p.Sign)
                .HasMaxLength(10)
                .IsRequired();

            builder.ToTable("Stages");
        }
    }
}

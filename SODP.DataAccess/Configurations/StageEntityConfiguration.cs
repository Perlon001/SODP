using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;

namespace SODP.DataAccess.Configurations
{
    public class StageEntityConfiguration : IEntityTypeConfiguration<Stage>
    {
        public void Configure(EntityTypeBuilder<Stage> builder)
        {
            builder.Property(p => p.Sign)
                .HasColumnType("varchar(10)")
                .IsRequired();

            builder.Property(p => p.Title)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            builder.ToTable("Stages");
        }
    }
}

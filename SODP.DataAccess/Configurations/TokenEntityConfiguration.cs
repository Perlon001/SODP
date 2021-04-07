using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SODP.Model;

namespace SODP.DataAccess.Configurations
{
    public class TokenEntityConfiguration : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.Ignore(p => p.Access);

            builder.Property(x => x.RefreshTokenKey)
                .HasColumnType("nvarchar(256)");

            builder.Property(x => x.Refresh)
                .HasColumnType("nvarchar(256)");

            builder.HasIndex(s => s.UserId)
                .HasName("IX_User");

            builder.ToTable("Tokens");

            //builder.HasOne("SODP.Model.User", "User")
            //    .WithMany()
            //    .HasForeignKey("UserId")
            //    .HasConstraintName("FK_User")
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne("WebSODP.Model.User", null)
            //    .WithMany("Tokens")
            //    .HasForeignKey("UserId")
            //    .HasConstraintName("FK_User");
        }
    }
}

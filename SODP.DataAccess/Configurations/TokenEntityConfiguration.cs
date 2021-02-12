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

            //builder.Property(p => p.Id)
            //    .HasColumnName("ID")
            //    .HasColumnType("Int")
            //    .UseMySqlIdentityColumn()
            //    .IsRequired();

            //builder.Property(p => p.UserId)
            //    .HasColumnName("TK_USID")
            //    .HasColumnType("Int")
            //    .IsRequired();

            //builder.Property(p => p.Refresh)
            //    .HasColumnName("TK_REFRESH")
            //    .IsRequired();

            //builder.Property(p => p.RefreshTokenKey)
            //    .HasColumnName("TK_KEYREF")
            //    .HasColumnType("VarChar(36)")
            //    .IsRequired();

            //builder.HasKey(s => s.Id);
            //.HasName("IX_KEY");

            builder.HasIndex(s => s.UserId)
                .HasName("IX_User");

            builder.ToTable("Tokens");

            builder.HasOne("SODP.Model.User", "User")
                .WithMany()
                .HasForeignKey("UserId")
                .HasConstraintName("FK_User")
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne("WebSODP.Model.User", null)
            //    .WithMany("Tokens")
            //    .HasForeignKey("UserId")
            //    .HasConstraintName("FK_User");
        }
    }
}

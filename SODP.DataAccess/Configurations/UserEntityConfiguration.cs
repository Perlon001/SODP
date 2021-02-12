using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SODP.Model;

namespace SODP.DataAccess.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // A concurrency token for use with the optimistic concurrency checking
            builder.Property(u => u.ConcurrencyStamp)
                .IsConcurrencyToken();

            builder.Property(u => u.UserName)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(u => u.NormalizedUserName)
                .HasMaxLength(256);

            builder.Property(u => u.Email)
                .HasMaxLength(256);

            builder.Property(u => u.NormalizedEmail)
                .HasMaxLength(256);

            builder.Property(u => u.Forename)
                .HasMaxLength(256);

            builder.Property(u => u.Surname)
                .HasMaxLength(256);

            builder.Property(u => u.ConcurrencyStamp)
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.UserName)
                .HasName("IX_UserName")
                .IsUnique();

            builder.ToTable("Users");

            // The relationships between User and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each User can have many UserClaims
            //builder.HasMany<IdentityUserClaim<int>>()
            //    .WithOne()
            //    .HasForeignKey(uc => uc.UserId)
            //    .IsRequired();

            // Each User can have many UserLogins
            //builder.HasMany</*IdentityUserLogin*/<int>>()
            //    .WithOne()
            //    .HasForeignKey(ul => ul.UserId)
            //    .IsRequired();

            // Each User can have many UserTokens
            //builder.HasMany<IdentityUserToken<int>>()
            //    .WithOne()
            //    .HasForeignKey(ut => ut.UserId)
            //    .IsRequired();

            // Each User can have many entries in the UserRole join table
            //builder.HasMany<Role>()
            //    .WithOne()
            //    .HasForeignKey(ur => ur.)
            //    .HasConstraintName("FK_RoleId")
            //    .IsRequired();


            // Each User can have many entries in the Token join table
            //builder.HasMany<Token>()
            //    .WithOne()
            //    .HasForeignKey(ur => ur.UserId)
            //    .HasConstraintName("FK_User")
            //    .IsRequired();

            //builder.HasOne("WebSODP.Model.User", null)
            //    .WithMany("Role")
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

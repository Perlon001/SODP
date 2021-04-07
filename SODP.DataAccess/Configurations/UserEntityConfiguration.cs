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
                .HasColumnType("nvarchar(256)")
                .IsRequired();

            builder.Property(u => u.NormalizedUserName)
                .HasColumnType("nvarchar(256)")
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnType("nvarchar(256)");

            builder.Property(u => u.NormalizedEmail)
                .HasColumnType("nvarchar(256)");

            builder.Property(u => u.Firstname)
                .HasColumnType("nvarchar(256)");

            builder.Property(u => u.Lastname)
                .HasColumnType("nvarchar(256)");

            builder.Property(u => u.PhoneNumber)
                .HasColumnType("varchar(256)");

            builder.Property(u => u.ConcurrencyStamp)
                .HasColumnType("varchar(256)")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

            builder.Property(u => u.PasswordHash)
                .HasColumnType("varchar(256)");

            builder.Property(u => u.SecurityStamp)
                .HasColumnType("varchar(256)");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.UserName)
                .HasName("IX_UserName")
                .IsUnique();

            builder.HasIndex(u => u.NormalizedUserName)
                .HasName("IX_MormalizedUserName")
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .HasName("IX_Email");

            builder.HasIndex(u => u.NormalizedEmail)
                .HasName("IX_NormalizedEmail");

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

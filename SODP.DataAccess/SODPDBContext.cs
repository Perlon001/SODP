using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SODP.DataAccess.Configurations;
using SODP.Model;

namespace SODP.DataAccess
{
    public class SODPDBContext : IdentityDbContext<User, Role, int>
    {
        public SODPDBContext(DbContextOptions<SODPDBContext> options) : base(options) { }
        
        public DbSet<Project> Projects { get; set; }
        public DbSet<Stage> Stages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new UserEntityConfiguration().Configure(modelBuilder.Entity<User>());
            new RoleEntityConfiguration().Configure(modelBuilder.Entity<Role>());

            new TokenEntityConfiguration().Configure(modelBuilder.Entity<Token>());

            new ProjectEntityConfiguration().Configure(modelBuilder.Entity<Project>());
            new StageEntityConfiguration().Configure(modelBuilder.Entity<Stage>());

            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "Admin", NormalizedName = "Admin".ToUpper() });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = "ProjectManager", NormalizedName = "ProjectManager".ToUpper() });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 3, Name = "User", NormalizedName = "User".ToUpper() });

            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(new User { Id = 1, UserName = "Admin", NormalizedUserName = "Admin".ToUpper(), PasswordHash = hasher.HashPassword(null, "Admin"), SecurityStamp = string.Empty });

            //modelBuilder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>
            //{
            //    RoleId = Roles.FirstOrDefaultAsync(x => x.NormalizedName == "Admin".ToUpper()).Id,
            //    UserId = Users.FirstOrDefaultAsync(x => x.NormalizedUserName == "Admin".ToUpper()).Id
            //});
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}

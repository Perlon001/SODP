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

        public DbSet<Stage> Stages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<ProjectBranch> ProjectBranches { get; set; }
        public DbSet<Designer> Designers { get; set; }
        public DbSet<Licence> Licences { get; set; }
        public DbSet<Certificate> Certificates { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new UserEntityConfiguration().Configure(modelBuilder.Entity<User>());
            new RoleEntityConfiguration().Configure(modelBuilder.Entity<Role>());

            new TokenEntityConfiguration().Configure(modelBuilder.Entity<Token>());

            new StageEntityConfiguration().Configure(modelBuilder.Entity<Stage>());
            new ProjectEntityConfiguration().Configure(modelBuilder.Entity<Project>());
            new BranchEntityConfiguration().Configure(modelBuilder.Entity<Branch>());
            new ProjectBranchEntityConfiguration().Configure(modelBuilder.Entity<ProjectBranch>());
            new DesignerEntityConfiguration().Configure(modelBuilder.Entity<Designer>());
            new LicenceEntityConfiguration().Configure(modelBuilder.Entity<Licence>());
            new CertificateEntityConfiguration().Configure(modelBuilder.Entity<Certificate>());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}

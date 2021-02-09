using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SODP.DataAccess
{
    public class MigrationContextFactory : IDesignTimeDbContextFactory<SODPDBContext>
    {
        public SODPDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SODPDBContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=SODP;Uid=www-data;Pwd=www-data;");

            return new SODPDBContext(optionsBuilder.Options);
        }
    }
}

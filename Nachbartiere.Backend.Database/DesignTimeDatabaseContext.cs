using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nachbartiere.Backend.Database
{
    internal class DesignTimeDatabaseContext : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var conStr = "Server=127.0.0.1;Port=5432;Database=nachbartiere;User Id=postgres;Password=postgres;";

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseNpgsql(conStr);

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
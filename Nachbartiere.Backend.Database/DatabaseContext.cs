using Microsoft.EntityFrameworkCore;
using Nachbartiere.Backend.Database.Entities;
using Nachbartiere.Backend.Database.Entities.Maps;

namespace Nachbartiere.Backend.Database
{
    public class DatabaseContext : DbContext
    {
        public static string? ConnectionString { get; set; }

        private static DbContextOptions<DatabaseContext> BuildOptions()
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();

            if (ConnectionString is null)
            {
                Console.WriteLine("Warning: Accessing DbContext before setting connection string.");
                return builder.Options;
            }

            builder.UseNpgsql(ConnectionString);

            return builder.Options;
        }

        internal DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DatabaseContext() : this(BuildOptions())
        {

        }

        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<Sighting> Sightings { get; set; }

        public DbSet<InviteToken> InviteTokens { get; set; }

        public DbSet<UserToTestGroup> UsersToTestGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AchievementMap());
            modelBuilder.ApplyConfiguration(new SightingMap());
            modelBuilder.ApplyConfiguration(new InviteTokenMap());
            modelBuilder.ApplyConfiguration(new UserToTestGroupMap());
        }
    }
}
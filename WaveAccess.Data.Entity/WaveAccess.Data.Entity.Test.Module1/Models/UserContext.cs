using System.Data.Entity;
using WaveAccess.Data.Entity.Test.Module1.Migrations;
using WaveAccess.Data.Entity.Test.Module1.Models.Mapping;

namespace WaveAccess.Data.Entity.Test.Module1.Models
{
    public class UserContext : DbContext
    {
        static UserContext()
        {
             Database.SetInitializer<UserContext>(new MigrateDatabaseToLatestVersion<UserContext, Configuration>());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new GroupMap());
            modelBuilder.Configurations.Add(new GroupHierarchyMap());
        }
    }
}


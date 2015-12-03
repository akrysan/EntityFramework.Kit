using System.Data.Entity;
using WaveAccess.Data.Entity.Test.Module1.Models;
using WaveAccess.Data.Entity.Test.Module1.Models.Mapping;
using WaveAccess.Data.Entity.Test.Module2.Migrations;
using WaveAccess.Data.Entity.Test.Module2.Models.Mapping;

namespace WaveAccess.Data.Entity.Test.Module2.Models
{
    public class TaskContext : DbContext
    {
        static TaskContext()
        {
            Database.SetInitializer<TaskContext>(new MigrateDatabaseToLatestVersion<TaskContext, Configuration>());
        }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new TaskMap());
            modelBuilder.Configurations.Add(new PkMappingDecorator<UserMap, User>());
        }
    }
}


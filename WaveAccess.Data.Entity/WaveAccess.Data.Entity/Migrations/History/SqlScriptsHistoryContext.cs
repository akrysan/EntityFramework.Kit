namespace WaveAccess.Data.Entity.Migrations.History {
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;


    public class SqlScriptsHistorEntity {
        public string ScriptName { get; set; }
        public string Hash { get; set; }
    }

    public class SqlScriptsHistoryContext:DbContext {
        public SqlScriptsHistoryContext() {
            Database.SetInitializer<SqlScriptsHistoryContext>(new MigrateDatabaseToLatestVersion<SqlScriptsHistoryContext, Configuration>(true));
        }

        public SqlScriptsHistoryContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection) {
        }
        public DbSet<SqlScriptsHistorEntity> SqlScriptsHistory { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            var historyMapping = modelBuilder.Entity<SqlScriptsHistorEntity>().ToTable("__MigrationSqlScriptHistory")
                .HasKey(h => new {h.ScriptName});

                historyMapping.Property(h => h.ScriptName).HasMaxLength(1048).IsRequired();
                historyMapping.Property(h => h.Hash).HasColumnType("char").HasMaxLength(32).IsRequired();
        }
    }

    internal sealed class Configuration : DbMigrationsConfiguration<SqlScriptsHistoryContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
        }
    }
}

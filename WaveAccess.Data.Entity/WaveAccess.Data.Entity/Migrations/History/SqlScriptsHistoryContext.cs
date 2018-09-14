namespace WaveAccess.Data.Entity.Migrations.History
{
    using System;
    using System.Data.Common;
    using System.Data.Entity;


    public class SqlScriptsHistorEntity {
        public string ScriptName { get; set; }
        public DateTime ExecutionDateUtc { get; set; }
        public string Hash { get; set; }
    }

    public class SqlScriptsHistoryContext:DbContext {

        static SqlScriptsHistoryContext() {
            Database.SetInitializer(new NullDatabaseInitializer<SqlScriptsHistoryContext>());
        }

        public SqlScriptsHistoryContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection) {
            Database.SetInitializer(new NullDatabaseInitializer<SqlScriptsHistoryContext>());
        }
        public DbSet<SqlScriptsHistorEntity> SqlScriptsHistory { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            var historyMapping = modelBuilder.Entity<SqlScriptsHistorEntity>().ToTable("__MigrationSqlScriptHistory")
                .HasKey(h => new {h.ScriptName});

                historyMapping.Property(h => h.ScriptName).HasMaxLength(1048).IsRequired();
                historyMapping.Property(h => h.ExecutionDateUtc).IsRequired();
                historyMapping.Property(h => h.Hash).HasColumnType("char").HasMaxLength(32).IsRequired();
        }
    }
}

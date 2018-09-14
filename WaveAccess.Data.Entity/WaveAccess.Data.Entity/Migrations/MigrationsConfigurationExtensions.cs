using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using WaveAccess.Data.Entity.Migrations.History;

namespace WaveAccess.Data.Entity.Migrations
{
    public static class MigrationsConfigurationExtensions {
        private static Regex _regex = new Regex(@"^\s*GO\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        private static string GetResourceCultureName() {

            var configCultureSection = System.Configuration.ConfigurationManager.GetSection("system.web/globalization");
            Type sectionType = configCultureSection.GetType();
            string configCultureName = sectionType.GetProperty("Culture").GetValue(configCultureSection).ToString();
            if (!string.IsNullOrWhiteSpace(configCultureName)) return configCultureName;

            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public static IEnumerable<SqlResourceInfo> ExecuteSqlScripts<TContext>(this DbMigrationsConfiguration<TContext> config, TContext context, string packName) where TContext : DbContext {
            return ExecuteSqlScripts(config, context, ScriptExecuteRule.ModifiedPack, packName);
        }

        internal static void ExecuteSqlScript(this SqlResourceInfo resource, SqlScriptsHistoryContext historyContext, DbContextTransaction packTran = null) {
            DbContextTransaction internalTran = null;
            if (packTran == null) internalTran = historyContext.Database.BeginTransaction();
            try {
                string[] commands = _regex
                    .Split(resource.GetSqlScript())
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .ToArray();
                foreach (var command in commands) {
                    historyContext.Database.ExecuteSqlCommand(command);
                }
                historyContext.SqlScriptsHistory.AddOrUpdate(h => h.ScriptName, new SqlScriptsHistorEntity() { ScriptName = resource.Path, Hash = resource.Hash, ExecutionDateUtc = DateTime.UtcNow });
                historyContext.SaveChanges();

                if (internalTran != null) internalTran.Commit();
            } finally {
                if (internalTran != null) internalTran.Dispose();
            }
        }

        public static IEnumerable<SqlResourceInfo> ExecuteSqlScripts<TContext>(this DbMigrationsConfiguration<TContext> config, TContext context, ScriptExecuteRule rule = ScriptExecuteRule.Modified, string packName = null) where TContext : DbContext {

            packName = (packName ?? String.Empty).Trim();
            var configType = config.GetType();
            var startString = configType.Namespace + SqlResourceInfo.scriptFolderName + (string.IsNullOrWhiteSpace(packName) ? "" : packName + ".");

            var migrateCultures = new[] { "Default", GetResourceCultureName() };

            var resources = configType.Assembly.GetManifestResourceNames().Where(s => s.StartsWith(startString, StringComparison.InvariantCultureIgnoreCase)
                                                                        && s.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase)
                ).Select(s => new SqlResourceInfo(configType, packName, s))
                .Where(r => migrateCultures.Contains(r.CultureName, StringComparer.InvariantCultureIgnoreCase));

            using (var historyContext = new SqlScriptsHistoryContext(context.Database.Connection, false)) {
                InitializeDatabase(historyContext);
                historyContext.Database.CommandTimeout = (int)Math.Max(context.Database.CommandTimeout ?? 0, 120);
                var histories = historyContext.SqlScriptsHistory.Where(h => h.ScriptName.StartsWith(startString)).ToDictionary(h => h.ScriptName.ToLower(), h => h.Hash);

                DbContextTransaction packTran = null;
                switch (rule) {
                    case ScriptExecuteRule.Modified:
                        resources = resources.Where(r => {
                            string hashVal;
                            return !histories.TryGetValue(r.Path.ToLower(), out hashVal) || !hashVal.Equals(r.Hash, StringComparison.InvariantCultureIgnoreCase);
                        });
                        break;
                    case ScriptExecuteRule.ModifiedPack:
                        if (!resources.Any(r => {
                            string hashVal;
                            return !histories.TryGetValue(r.Path.ToLower(), out hashVal) || !hashVal.Equals(r.Hash, StringComparison.InvariantCultureIgnoreCase);
                        })) {
                            resources = new SqlResourceInfo[] { };
                        } else {
                            packTran = historyContext.Database.BeginTransaction();
                        }
                        break;
                    case ScriptExecuteRule.Once:
                        resources = resources.Where(r => !histories.ContainsKey(r.Path.ToLower()));
                        break;
                    //default: // do nothing on ScriptExecuteRule.Always
                }

                resources = resources
                        .OrderBy(r => r.CultureName == "Default" ? 0 : 1)
                        .ThenBy(r => r.FolderName + "z")
                        .ThenBy(r => r.Path).ToArray();
                try {
                    foreach (var resourceInfo in resources.ToArray()) {
                        resourceInfo.ExecuteSqlScript(historyContext, packTran);
                    }
                    if (packTran != null) packTran.Commit();
                } finally {
                    if (packTran != null) packTran.Dispose();
                }
                return resources;
            }
        }

        private static void InitializeDatabase(DbContext dbContext) {
            dbContext.Database.ExecuteSqlCommand(@"IF OBJECT_ID(N'[dbo].[__MigrationSqlScriptHistory]', N'U') IS NULL
CREATE TABLE [dbo].[__MigrationSqlScriptHistory] (
    [ScriptName]       NVARCHAR (1048) NOT NULL,
    [ExecutionDateUtc] DATETIME        NOT NULL,
    [Hash]             CHAR (32)       NOT NULL
);");
        }
    }
}

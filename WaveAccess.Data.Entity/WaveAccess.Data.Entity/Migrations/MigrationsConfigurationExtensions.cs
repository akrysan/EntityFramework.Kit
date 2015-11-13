using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WaveAccess.Data.Entity.Migrations.History;

namespace WaveAccess.Data.Entity.Migrations {
    public static class MigrationsConfigurationExtensions {

        private static string GetResourceCultureName() {

            var configCultureSection = System.Configuration.ConfigurationManager.GetSection("system.web/globalization");
            Type sectionType = configCultureSection.GetType();
            string configCultureName = sectionType.GetProperty("Culture").GetValue(configCultureSection).ToString();
            if (!string.IsNullOrWhiteSpace(configCultureName)) return configCultureName;

            return Thread.CurrentThread.CurrentCulture.Name;
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
                historyContext.Database.Initialize(true);
                historyContext.Database.CommandTimeout = 60;
                var histories = historyContext.SqlScriptsHistory.Where(h => h.ScriptName.StartsWith(startString)).ToDictionary(h => h.ScriptName.ToLower(), h => h.Hash);

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

                foreach (var resourceInfo in resources.ToArray()) {
                    resourceInfo.ExecuteSqlScript(historyContext);
                }
                return resources;
            }
        }
    }
}

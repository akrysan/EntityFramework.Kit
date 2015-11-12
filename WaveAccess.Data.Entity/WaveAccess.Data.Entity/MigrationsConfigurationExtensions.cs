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
using System.Threading;
using System.Threading.Tasks;
using WaveAccess.Data.Entity.Migrations.History;

namespace WaveAccess.Data.Entity {
    public static class MigrationsConfigurationExtensions {
        private const string scriptFolderName = ".SqlScripts.";

        private class SqlResourceInfo {
            public SqlResourceInfo(Type configType, string Path) {
                this.Assembly = configType.Assembly;
                this.Path = Path;
                _namespaceLength = configType.Namespace.Length + scriptFolderName.Length;
            }

            private static HashAlgorithm _algorithm = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5"));

            public Assembly Assembly { get; private set; }
            public string Path { get; private set; }
            private int _namespaceLength;

            private string _hash;

            public string Hash {
                get {
                    return _hash ?? (_hash = CalculateHash());
                }
            }

            private string CalculateHash() {
                using (var resStream = GetResourceStream()) {
                    byte[] hashBytes = _algorithm.ComputeHash(resStream);
                    resStream.Close();
                    StringBuilder sb = new StringBuilder(hashBytes.Length * 2);
                    for (int i = 0; i < hashBytes.Length; i++) {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
            }

            private string _culture;
            public string CultureName {
                get {
                    return _culture ?? (_culture = Path.Substring(_namespaceLength, Path.IndexOf('.', _namespaceLength) - _namespaceLength).Replace('_', '-'));
                }
            }

            private string _folderName;
            public string FolderName {
                get {
                    return _folderName ?? (_folderName = Path.Substring(0, Path.LastIndexOf('.', Path.Length - 5)));
                }
            }

            private string _setName;
            public string SetName {
                get {
                    return _setName ?? (_setName = FolderName.EndsWith(".set", StringComparison.InvariantCultureIgnoreCase) ? FolderName : Path);
                }
            }

            private Stream GetResourceStream() {
                return Assembly.GetManifestResourceStream(Path);
            }

            private string GetSqlScript() {
                using (var resStream = GetResourceStream())
                using (var textStream = new StreamReader(resStream)) {
                    return textStream.ReadToEnd();
                }
            }

            public void ExecuteSqlScript(SqlScriptsHistoryContext historyContext) {
                using (var tran = historyContext.Database.BeginTransaction()) {

                    historyContext.Database.ExecuteSqlCommand(this.GetSqlScript());
                    historyContext.SqlScriptsHistory.AddOrUpdate(h => h.ScriptName, new SqlScriptsHistorEntity() { ScriptName = this.Path, Hash = this.Hash, ExecutionDateUtc = DateTime.UtcNow });
                    historyContext.SaveChanges();

                    tran.Commit();
                }
            }
        }

        private static string GetResourceCultureName(string cultureName) {
            if (!string.IsNullOrWhiteSpace(cultureName)) return cultureName;

            var configCultureSection = System.Configuration.ConfigurationManager.GetSection("system.web/globalization");
            Type sectionType = configCultureSection.GetType();
            string configCultureName = sectionType.GetProperty("Culture").GetValue(configCultureSection).ToString();
            if (!string.IsNullOrWhiteSpace(configCultureName)) return configCultureName;

            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public static void ExecuteSqlScripts<TContext>(this DbMigrationsConfiguration<TContext> config, TContext context, string cultureName = null) where TContext : DbContext {

            var configType = config.GetType();
            var startString = configType.Namespace + scriptFolderName;

            var migrateCultures = new[] { "Default", GetResourceCultureName(cultureName) };

            var resources = configType.Assembly.GetManifestResourceNames().Where(s => s.StartsWith(startString, StringComparison.InvariantCultureIgnoreCase)
                                                                        && s.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase)
                ).Select(s => new SqlResourceInfo(configType, s))
                .Where(r => migrateCultures.Contains(r.CultureName, StringComparer.InvariantCultureIgnoreCase))
                .ToArray();

            using (var historyContext = new SqlScriptsHistoryContext(context.Database.Connection, false)) {
                historyContext.Database.Initialize(true);
                historyContext.Database.CommandTimeout = 60;
                var histories = historyContext.SqlScriptsHistory.Where(h => h.ScriptName.StartsWith(startString)).ToDictionary(h => h.ScriptName.ToLower(), h => h.Hash);

                var resourcesSets = resources.GroupBy(r => r.SetName).ToDictionary(g => g.Key, g => g.ToArray());
                resources = resources.Where(r => resourcesSets[r.SetName].Any(rs => {
                    string hashVal;
                    return !histories.TryGetValue(rs.Path.ToLower(), out hashVal) || !hashVal.Equals(rs.Hash, StringComparison.InvariantCultureIgnoreCase);
                }))
                    .OrderBy(r => r.CultureName == "Default" ? 0 : 1)
                    .ThenBy(r => r.FolderName)
                    .ThenBy(r => r.Path).ToArray();

                foreach (var resourceInfo in resources) {
                    resourceInfo.ExecuteSqlScript(historyContext);
                }
            }
        }
    }
}

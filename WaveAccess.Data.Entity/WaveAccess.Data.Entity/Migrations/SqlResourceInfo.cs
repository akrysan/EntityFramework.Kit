using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WaveAccess.Data.Entity.Migrations.History;
using System.Data.Entity.Migrations;

namespace WaveAccess.Data.Entity.Migrations {
    public class SqlResourceInfo {
        
        internal const string scriptFolderName = ".SqlScripts.";
        private static Regex _regex = new Regex(@"^\s*GO\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        internal SqlResourceInfo(Type configType, string packName, string Path) {
            this.Assembly = configType.Assembly;
            this.Path = Path;
            this.PackName = packName;
            _namespaceLength = configType.Namespace.Length + scriptFolderName.Length +(packName.Length == 0 ? 0 : packName.Length + 1);
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

        public string PackName { get; private set; }

        private Stream GetResourceStream() {
            return Assembly.GetManifestResourceStream(Path);
        }

        private string GetSqlScript() {
            using (var resStream = GetResourceStream())
            using (var textStream = new StreamReader(resStream)) {
                return textStream.ReadToEnd();
            }
        }

        internal void ExecuteSqlScript(SqlScriptsHistoryContext historyContext) {
            using (var tran = historyContext.Database.BeginTransaction()) {
                string[] commands = _regex.Split(this.GetSqlScript());
                foreach (var command in commands) {
                    historyContext.Database.ExecuteSqlCommand(command);
                }
                historyContext.SqlScriptsHistory.AddOrUpdate(h => h.ScriptName, new SqlScriptsHistorEntity() { ScriptName = this.Path, Hash = this.Hash, ExecutionDateUtc = DateTime.UtcNow });
                historyContext.SaveChanges();

                tran.Commit();
            }
        }
    }
}

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
   
        internal SqlResourceInfo(Type configType, string packName, string Path) {
            this.Assembly = configType.Assembly;
            this.Path = Path;
            this.PackName = packName;
            _namespaceLength = configType.Namespace.Length + scriptFolderName.Length +(packName.Length == 0 ? 0 : packName.Length + 1);
        }

        [ThreadStatic]
        private static HashAlgorithm _algorithm;

        private  Assembly Assembly { get; set; }
        public string Path { get; private set; }
        private int _namespaceLength;

        private string _hash;

        public string Hash {
            get {
                return _hash ?? (_hash = CalculateHash());
            }
        }

        private HashAlgorithm Algorithm
        {
            get
            {
                if (_algorithm == null)
                {
                    _algorithm = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5"));
                }

                return _algorithm;
            }
        }

        private string CalculateHash() {
            using (var resStream = GetResourceStream()) {
                byte[] hashBytes = Algorithm.ComputeHash(resStream);
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

        internal string GetSqlScript() {
            using (var resStream = GetResourceStream())
            using (var textStream = new StreamReader(resStream)) {
                return textStream.ReadToEnd();
            }
        }
    }
}

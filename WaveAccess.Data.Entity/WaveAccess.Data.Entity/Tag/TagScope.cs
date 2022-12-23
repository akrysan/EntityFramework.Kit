namespace WaveAccess.Data.Entity.Tag {
    using System;
    using System.Data.Common;
    public class TagScope : IDisposable {
        [ThreadStatic]
        internal static TagScope CurrentTagScope;

        public string QueryTag { get; private set; }

        public void Dispose() => CurrentTagScope = null;

        public TagScope(string tag) {
            QueryTag = !string.IsNullOrWhiteSpace(tag) ? tag : throw new ArgumentOutOfRangeException(nameof(tag));
            CurrentTagScope = this;
        }

        protected internal virtual void ApplyTag(DbCommand command) {
            command.CommandText = string.Format("/*{0}*/ ", QueryTag) + command.CommandText;
        }
    }
}

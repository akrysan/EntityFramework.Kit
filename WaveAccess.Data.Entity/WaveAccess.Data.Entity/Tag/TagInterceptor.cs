namespace WaveAccess.Data.Entity.Tag {
    using System.Data.Common;
    using System.Data.Entity.Infrastructure.Interception;
    public class TagInterceptor : DbCommandInterceptor {
        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) {
            if (TagScope.CurrentTagScope != null) {
                TagScope.CurrentTagScope.ApplyTag(command);
            }

            base.ReaderExecuting(command, interceptionContext);
        }

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) {
            if (TagScope.CurrentTagScope != null) {
                TagScope.CurrentTagScope.ApplyTag(command);
            }

            base.ScalarExecuting(command, interceptionContext);
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) {
            if (TagScope.CurrentTagScope != null) {
                TagScope.CurrentTagScope.ApplyTag(command);
            }

            base.ScalarExecuted(command, interceptionContext);
        }
    }
}

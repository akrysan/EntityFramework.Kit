using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity.Hint
{
    public class HintInterceptor : DbCommandInterceptor
    {
        private void EditCommand<T>(DbCommand command, DbCommandInterceptionContext<T> interceptionContext)
        {
            if (interceptionContext.DbContexts.Any(db => db is IQueryHintContext))
            {
                var ctx = interceptionContext.DbContexts.First(db => db is IQueryHintContext) as IQueryHintContext;
                if (ctx.ApplyHint)
                {
                    command.CommandText += string.Format(" OPTION ({0})", ctx.QueryHint);
                }
            }
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            EditCommand(command, interceptionContext);
            base.ReaderExecuting(command, interceptionContext);
        }

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            EditCommand(command, interceptionContext);
            base.ScalarExecuting(command, interceptionContext);
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            EditCommand(command, interceptionContext);
            base.ScalarExecuted(command, interceptionContext);
        }
    }
}

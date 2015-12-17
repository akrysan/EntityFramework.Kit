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
        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            if (HintScope.CurrentHintScope != null)
            {
                HintScope.CurrentHintScope.ApplyHint(command, interceptionContext);
            }
            base.ReaderExecuting(command, interceptionContext);
        }

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            if (HintScope.CurrentHintScope != null)
            {
                HintScope.CurrentHintScope.ApplyHint(command, interceptionContext);
            }
            base.ScalarExecuting(command, interceptionContext);
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            if (HintScope.CurrentHintScope != null)
            {
                HintScope.CurrentHintScope.ApplyHint(command, interceptionContext);
            }
            base.ScalarExecuted(command, interceptionContext);
        }
    }
}

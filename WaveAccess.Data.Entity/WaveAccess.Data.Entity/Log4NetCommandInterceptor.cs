using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WaveAccess.Data.Entity
{
 
    public class Log4NetCommandInterceptor : IDbCommandInterceptor
    {
        private static ILog Log = LogManager.GetLogger("EntityFramework.SQL");

        private readonly Stopwatch _stopwatch = new Stopwatch();
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            _stopwatch.Restart();
        }
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            _stopwatch.Stop();
            LogCommand(command, interceptionContext);
        }
        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            _stopwatch.Restart();
        }
        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            _stopwatch.Stop();
            LogCommand(command, interceptionContext);
        }
        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            _stopwatch.Restart();
        }
        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            _stopwatch.Stop();
            LogCommand(command, interceptionContext);
        }
        private void LogCommand<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            Log.LogCommand(command, _stopwatch.ElapsedMilliseconds, ((object)interceptionContext.OriginalResult??"(Null)").ToString(), interceptionContext.Exception);
        }
    }
}

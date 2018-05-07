using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WaveAccess.Data.Entity
{
    public static class Log4NetSQLExtension
    {
        public static void LogCommand(this ILog log, DbCommand command, long duration, object result, Exception exception = null)
        {
            // Ignore EF internal exceptions
            if (command != null && command.CommandText != null && command.CommandText.Contains("[dbo].[__MigrationHistory]") &&
                exception != null && exception.Message.Contains("Invalid object name 'dbo.__MigrationHistory'"))
            {
                return;
            }

            if (command != null && command.CommandText != null && command.CommandText.Contains("[dbo].[__MigrationHistory]") &&
                exception != null && exception.Message.Contains("Invalid column name 'CreatedOn'"))
            {
                return;
            }

            if (command != null && command.CommandText != null && command.CommandText.Contains("[dbo].[EdmMetadata]") &&
                exception != null && exception.Message.Contains("Invalid object name 'dbo.EdmMetadata'"))
            {
                return;
            }

            AddExtendedThreadInfo();
            if (exception != null)
            {
                log.ErrorFormat(" Exception occurred when executing the command:{0} {1} {0} Exception:{0}{2}",
                    Environment.NewLine,
                    CreateCommandMessage(command),
                    exception);
            }
            else
            {
                var logEvent = new log4net.Core.LoggingEvent(typeof(Log4NetCommandInterceptor), log.Logger.Repository, log.Logger.Name, Level.Debug, CreateCommandMessage(command), exception);
                logEvent.Properties["Duration"] = duration;
                logEvent.Properties["Result"] = result;
                log.Logger.Log(logEvent);
            }

        }

        private static void AddExtendedThreadInfo()
        {
            try
            {
                var ctx = HttpContext.Current;
                if (ctx != null)
                {
                    ThreadContext.Properties["Url"] = ctx.Request.Url.ToString();
                    ThreadContext.Properties["Request"] = ctx.Request.RequestContext.GetHashCode();
                }
            }
            catch (HttpException)
            {
                // can happen on cloud service for an unknown reason
            }
        }

        private static string CreateCommandMessage(DbCommand command)
        {
            StringBuilder result = new StringBuilder();
            result.Append(command.CommandText);
            if (command.Parameters.Count > 0)
            {
                foreach (DbParameter p in command.Parameters)
                {
                    result.AppendLine();
                    result.AppendFormat("-- {0} = {1}", p.ParameterName, p.Value);
                }
            }
            return result.ToString();
        }
    }
}

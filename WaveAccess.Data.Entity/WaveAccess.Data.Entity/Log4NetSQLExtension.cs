using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Core;

namespace WaveAccess.Data.Entity
{
    public static class Log4NetSQLExtension
    {
        private static FieldInfo _messageFieldInfo = typeof(SqlException).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance);

        public static string GetMessage(this DbCommand command)
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

        public static void LogCommand(this ILog log, DbCommand command, long duration, object result)
        {
            if (log.IsDebugEnabled)
            {
                var data = new LoggingEventData
                {
                    LoggerName = log.Logger.Name,
                    Level = Level.Debug,
                    Message = command.GetMessage()
                };
                var logEvent = new LoggingEvent(typeof(Log4NetCommandInterceptor), log.Logger.Repository, data);
                logEvent.Properties["Duration"] = duration;
                logEvent.Properties["Result"] = result;
                log.Logger.Log(logEvent);
            }
        }

        public static void UpdateMessage(this SqlException sqlException, DbCommand command)
        {
            _messageFieldInfo.SetValue(sqlException, $"{sqlException.Message}{Environment.NewLine}SQL Command:{Environment.NewLine}{command.GetMessage()}");
        }
    }
}

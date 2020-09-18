using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity.Hint
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of Entity </typeparam>
    public class JoinHintScope<T> : HintScope where T : class
    {
        public JoinHintScope(string hint):base(hint)
        {

        }

        protected internal override void ApplyHint<TContext>(DbCommand command, DbCommandInterceptionContext<TContext> interceptionContext)
        {
            var oc = interceptionContext.ObjectContexts.First();
            var tableName = oc.GetTableName<T>();
            command.CommandText = Regex.Replace(command.CommandText,  $"JOIN\\s+{Regex.Escape(tableName)}", $"{QueryHint} JOIN {tableName}",
                                      RegexOptions.IgnoreCase|RegexOptions.Multiline);
        }
    }
}

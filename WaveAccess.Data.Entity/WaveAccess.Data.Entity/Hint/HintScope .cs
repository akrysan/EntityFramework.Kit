using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity.Hint
{
    public class HintScope : IDisposable
    {
        [ThreadStatic]
        internal static HintScope CurrentHintScope;

        public string QueryHint { get; private set; }

        public void Dispose()
        {
            CurrentHintScope = null;
        }

        public HintScope(string hint)
        {
            if (String.IsNullOrWhiteSpace(hint))
            {
                throw new ArgumentOutOfRangeException("hint");
            }

            QueryHint = hint;

            CurrentHintScope = this;
        }

        virtual internal protected void ApplyHint<T>(DbCommand command, DbCommandInterceptionContext<T> interceptionContext)
        {
            command.CommandText += string.Format(" OPTION ({0})", QueryHint);
        }
    }
}

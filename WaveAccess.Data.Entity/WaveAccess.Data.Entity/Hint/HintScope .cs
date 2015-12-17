using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity.Hint
{
    public class HintScope : IDisposable
    {
        public IQueryHintContext Context { get; private set; }
        public void Dispose()
        {
            Context.ApplyHint = false;
        }

        public HintScope(IQueryHintContext context, string hint)
        {
            Context = context;
            Context.ApplyHint = true;
            Context.QueryHint = hint;
        }
    }
}

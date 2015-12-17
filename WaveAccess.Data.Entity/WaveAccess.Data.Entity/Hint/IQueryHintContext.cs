using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity.Hint
{
    public interface IQueryHintContext
    {
        string QueryHint { get; set; }
        bool ApplyHint { get; set; }
    }
}

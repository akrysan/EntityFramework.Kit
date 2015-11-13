using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity.Migrations {
    public enum ScriptExecuteRule {
        Modified,
        ModifiedPack,
        Once,
        Always        
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity
{
    public class SpecialDBConfiguration : DbConfiguration
    {
        public SpecialDBConfiguration()
        {
            this.AddDependencyResolver(new DBInitializerResolver());
        }
    }
}

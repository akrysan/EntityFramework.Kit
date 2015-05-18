using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WA.Data.Entity
{
    public class EFConfiguration : DbConfiguration
    {
        public EFConfiguration()
        {
            this.AddDependencyResolver(new DBInitializerResolver());
        }
    }
}

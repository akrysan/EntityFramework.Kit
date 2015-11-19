using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveAccess.Data.Entity.Test.SecondContext;

namespace WaveAccess.Data.Entity.Test.Models {
    public class SimpleEntity {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public virtual User User { get; set; }
    }
}

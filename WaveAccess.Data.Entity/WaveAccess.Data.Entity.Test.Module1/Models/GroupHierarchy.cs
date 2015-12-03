using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity.Test.Module1.Models
{
    public class GroupHierarchy
    {
        public int ChildId { get; set; }
        public int ParentId { get; set; }
        public int Depth { get; set; }
        public int? RootId { get; set; }
        public int? Level { get; set; }

        public virtual Group Child { get; set; }
        public virtual Group Parent { get; set; }
    }
}

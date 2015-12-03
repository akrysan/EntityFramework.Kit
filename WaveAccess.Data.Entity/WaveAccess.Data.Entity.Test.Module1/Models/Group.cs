using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity.Test.Module1.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentGroupId { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<GroupHierarchy> Parents { get; set; }
        public virtual ICollection<GroupHierarchy> Children { get; set; }
    }
}

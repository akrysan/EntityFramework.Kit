using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveAccess.Data.Entity.Test.Module1.Models;

namespace WaveAccess.Data.Entity.Test.Module2.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity.Test.SecondContext.Map {
    public class UserMap : EntityTypeConfiguration<User> {
        public UserMap() {
            this.ToTable("SystemUsers", "Sec");
            this.HasKey(u => u.Id);
            this.Property(u => u.Name).HasMaxLength(64);
        }
    }
}

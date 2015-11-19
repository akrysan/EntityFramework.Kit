using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveAccess.Data.Entity.Test.SecondContext;
using WaveAccess.Data.Entity.Test.SecondContext.Map;
using WaveAccess.Data.Entity.Test.SecondContext.Migrations;

namespace WaveAccess.Data.Entity.Test.SecondContext {
    public class SecondContext:DbContext {
        static SecondContext() {
            Database.SetInitializer<SecondContext>(new MigrateDatabaseToLatestVersion<SecondContext, Configuration>());            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}

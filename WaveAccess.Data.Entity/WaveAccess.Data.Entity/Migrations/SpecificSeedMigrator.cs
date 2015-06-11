using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity.Migrations
{
    public class SpecialSeedMigrator : MigratorBase
    {
        public SpecialSeedMigrator(DbMigrationsConfiguration configuration, DbContext context)
            : base((DbMigrator)Activator.CreateInstance(typeof(DbMigrator),
                BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { configuration, context }, null)) { }

        public override void Update(string targetMigration)
        {
            if (targetMigration == null)
            {
                if (GetPendingMigrations().Any())
                {
                    base.Update(targetMigration);
                }
            }
            else
            {
                base.Update(targetMigration);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WA.Data.Entity.Migrations
{
    public class MigratorSeedIfSchemaIsChanged : MigratorBase
    {
        public MigratorSeedIfSchemaIsChanged(DbMigrationsConfiguration configuration) : base(new DbMigrator(configuration)) { }

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

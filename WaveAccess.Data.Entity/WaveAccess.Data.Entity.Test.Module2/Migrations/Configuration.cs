namespace WaveAccess.Data.Entity.Test.Module2.Migrations
{
    using Entity.Migrations;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WaveAccess.Data.Entity.Test.Module2.Models.TaskContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WaveAccess.Data.Entity.Test.Module2.Models.TaskContext context)
        {
            this.ExecuteSqlScripts(context);
        }
    }
}

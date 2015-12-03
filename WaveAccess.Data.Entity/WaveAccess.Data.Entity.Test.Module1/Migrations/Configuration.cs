namespace WaveAccess.Data.Entity.Test.Module1.Migrations
{
    using Entity.Migrations;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WaveAccess.Data.Entity.Test.Module1.Models.UserContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WaveAccess.Data.Entity.Test.Module1.Models.UserContext context)
        {
            this.ExecuteSqlScripts(context);
        }
    }
}

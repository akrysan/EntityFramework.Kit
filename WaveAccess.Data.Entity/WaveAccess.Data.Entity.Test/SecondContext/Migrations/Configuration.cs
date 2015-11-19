namespace WaveAccess.Data.Entity.Test.SecondContext.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using WaveAccess.Data.Entity.Migrations;
    using WaveAccess.Data.Entity.Test.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SecondContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SecondContext context)
        {
              this.ExecuteSqlScripts(context);
        }
    }
}

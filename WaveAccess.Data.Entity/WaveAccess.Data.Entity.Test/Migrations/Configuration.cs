namespace WaveAccess.Data.Entity.Test.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using WaveAccess.Data.Entity.Migrations;
    using WaveAccess.Data.Entity.Test.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SimpleContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WaveAccess.Data.Entity.Test.Models.SimpleContext context)
        {
            this.ExecuteSqlScripts(context, ScriptExecuteRule.Once, "PreDeploy");
            this.ExecuteSqlScripts(context);
            var executedScripts = this.ExecuteSqlScripts(context, ScriptExecuteRule.ModifiedPack, "ScriptPack"); // can use this.ExecuteSqlScripts(context, "ScriptPack");
            if (executedScripts.Any(r => r.CultureName == "en-AU")) this.ExecuteSqlScripts(context, ScriptExecuteRule.Always, "PostDeploy");
        }
    }
}

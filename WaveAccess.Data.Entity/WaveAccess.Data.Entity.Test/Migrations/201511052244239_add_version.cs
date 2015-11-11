namespace WaveAccess.Data.Entity.Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_version : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SimpleEntities", "Version", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SimpleEntities", "Version");
        }
    }
}

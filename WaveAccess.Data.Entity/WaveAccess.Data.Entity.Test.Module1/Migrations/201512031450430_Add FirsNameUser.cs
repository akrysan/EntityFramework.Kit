namespace WaveAccess.Data.Entity.Test.Module1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFirsNameUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FirstName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "FirstName");
        }
    }
}

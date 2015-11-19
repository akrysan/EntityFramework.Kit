namespace WaveAccess.Data.Entity.Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userRef : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SimpleEntities", "User_Id", c => c.Int());
            CreateIndex("dbo.SimpleEntities", "User_Id");
            AddForeignKey("dbo.SimpleEntities", "User_Id", "Sec.SystemUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SimpleEntities", "User_Id", "Sec.SystemUsers");
            DropIndex("dbo.SimpleEntities", new[] { "User_Id" });
            DropColumn("dbo.SimpleEntities", "User_Id");
        }
    }
}

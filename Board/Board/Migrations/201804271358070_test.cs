namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ads", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Ads", "User_Id");
            AddForeignKey("dbo.Ads", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ads", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Ads", new[] { "User_Id" });
            DropColumn("dbo.Ads", "User_Id");
        }
    }
}

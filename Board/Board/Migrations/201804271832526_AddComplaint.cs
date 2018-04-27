namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComplaint : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Complaints",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Ads_Id = c.Guid(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ads", t => t.Ads_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Ads_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Complaints", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Complaints", "Ads_Id", "dbo.Ads");
            DropIndex("dbo.Complaints", new[] { "User_Id" });
            DropIndex("dbo.Complaints", new[] { "Ads_Id" });
            DropTable("dbo.Complaints");
        }
    }
}

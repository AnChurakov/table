namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageAdsSubCat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageAds",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Path = c.String(),
                        Ads_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ads", t => t.Ads_Id)
                .Index(t => t.Ads_Id);
            
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Ads", "SubCategory_Id", c => c.Guid());
            CreateIndex("dbo.Ads", "SubCategory_Id");
            AddForeignKey("dbo.Ads", "SubCategory_Id", "dbo.SubCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ads", "SubCategory_Id", "dbo.SubCategories");
            DropForeignKey("dbo.ImageAds", "Ads_Id", "dbo.Ads");
            DropIndex("dbo.ImageAds", new[] { "Ads_Id" });
            DropIndex("dbo.Ads", new[] { "SubCategory_Id" });
            DropColumn("dbo.Ads", "SubCategory_Id");
            DropTable("dbo.SubCategories");
            DropTable("dbo.ImageAds");
        }
    }
}

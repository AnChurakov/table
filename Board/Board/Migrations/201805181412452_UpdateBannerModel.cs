namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBannerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Banners", "SinglePage", c => c.Boolean(nullable: false));
            DropColumn("dbo.ImageBanners", "SinglePage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ImageBanners", "SinglePage", c => c.Boolean(nullable: false));
            DropColumn("dbo.Banners", "SinglePage");
        }
    }
}

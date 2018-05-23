namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBanners : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ImageBanners",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SinglePage = c.Boolean(nullable: false),
                        Banners_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banners", t => t.Banners_Id)
                .Index(t => t.Banners_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImageBanners", "Banners_Id", "dbo.Banners");
            DropIndex("dbo.ImageBanners", new[] { "Banners_Id" });
            DropTable("dbo.ImageBanners");
            DropTable("dbo.Banners");
        }
    }
}

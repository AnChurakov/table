namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinkBanner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Banners", "Link", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Banners", "Link");
        }
    }
}

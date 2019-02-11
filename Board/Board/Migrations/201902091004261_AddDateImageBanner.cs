namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateImageBanner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageBanners", "DateCreate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageBanners", "DateCreate");
        }
    }
}

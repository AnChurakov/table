namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameBanner : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Banners", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Banners", "Name", c => c.String());
        }
    }
}

namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescBanner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Banners", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Banners", "Description");
        }
    }
}

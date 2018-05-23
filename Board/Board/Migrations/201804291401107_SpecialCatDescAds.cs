namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpecialCatDescAds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ads", "Description", c => c.String());
            AddColumn("dbo.Categories", "Special", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "Special");
            DropColumn("dbo.Ads", "Description");
        }
    }
}

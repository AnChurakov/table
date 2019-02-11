namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateBanner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Banners", "DateCreate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Banners", "DateCreate");
        }
    }
}

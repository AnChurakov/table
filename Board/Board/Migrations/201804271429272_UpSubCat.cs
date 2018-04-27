namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpSubCat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubCategories", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubCategories", "Name", c => c.String());
        }
    }
}

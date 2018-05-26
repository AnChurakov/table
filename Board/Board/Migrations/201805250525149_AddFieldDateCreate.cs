namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldDateCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "DateCreate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "DateCreate");
        }
    }
}

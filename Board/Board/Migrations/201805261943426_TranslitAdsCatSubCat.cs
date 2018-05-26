namespace Board.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TranslitAdsCatSubCat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ads", "Transliteration", c => c.String());
            AddColumn("dbo.Categories", "Transliteration", c => c.String());
            AddColumn("dbo.SubCategories", "Transliteration", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubCategories", "Transliteration");
            DropColumn("dbo.Categories", "Transliteration");
            DropColumn("dbo.Ads", "Transliteration");
        }
    }
}

namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class purchasenotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchases", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purchases", "Notes");
        }
    }
}

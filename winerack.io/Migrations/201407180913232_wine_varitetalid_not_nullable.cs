namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wine_varitetalid_not_nullable : DbMigration
    {
        public override void Up()
        {
			//AlterColumn("dbo.Wines", "VarietalID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
			//AlterColumn("dbo.Wines", "VarietalID", c => c.Int(nullable: true));

        }
    }
}

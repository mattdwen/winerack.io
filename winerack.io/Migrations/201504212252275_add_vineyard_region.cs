namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_vineyard_region : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vineyards", "Region_ID", c => c.Int());
            CreateIndex("dbo.Vineyards", "Region_ID");
            AddForeignKey("dbo.Vineyards", "Region_ID", "dbo.Regions", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vineyards", "Region_ID", "dbo.Regions");
            DropIndex("dbo.Vineyards", new[] { "Region_ID" });
            DropColumn("dbo.Vineyards", "Region_ID");
        }
    }
}

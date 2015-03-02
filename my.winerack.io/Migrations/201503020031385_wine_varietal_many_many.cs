namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wine_varietal_many_many : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Wines", "VarietalID", "dbo.Varietals");
            DropIndex("dbo.Wines", new[] { "VarietalID" });
            CreateTable(
                "dbo.WineVarietals",
                c => new
                    {
                        Wine_ID = c.Int(nullable: false),
                        Varietal_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Wine_ID, t.Varietal_ID })
                .ForeignKey("dbo.Wines", t => t.Wine_ID, cascadeDelete: true)
                .ForeignKey("dbo.Varietals", t => t.Varietal_ID, cascadeDelete: true)
                .Index(t => t.Wine_ID)
                .Index(t => t.Varietal_ID);
            
            DropColumn("dbo.Wines", "VarietalID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Wines", "VarietalID", c => c.Int(nullable: false));
            DropForeignKey("dbo.WineVarietals", "Varietal_ID", "dbo.Varietals");
            DropForeignKey("dbo.WineVarietals", "Wine_ID", "dbo.Wines");
            DropIndex("dbo.WineVarietals", new[] { "Varietal_ID" });
            DropIndex("dbo.WineVarietals", new[] { "Wine_ID" });
            DropTable("dbo.WineVarietals");
            CreateIndex("dbo.Wines", "VarietalID");
            AddForeignKey("dbo.Wines", "VarietalID", "dbo.Varietals", "ID", cascadeDelete: true);
        }
    }
}

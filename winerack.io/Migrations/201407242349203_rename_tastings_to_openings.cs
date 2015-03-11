namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename_tastings_to_openings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tastings", "StoredBottleID", "dbo.StoredBottles");
            DropIndex("dbo.Tastings", new[] { "StoredBottleID" });
            CreateTable(
                "dbo.Openings",
                c => new
                    {
                        StoredBottleID = c.Int(nullable: false),
                        OpenedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ImageID = c.Guid(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.StoredBottleID)
                .ForeignKey("dbo.StoredBottles", t => t.StoredBottleID)
                .Index(t => t.StoredBottleID);
            
            DropTable("dbo.Tastings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tastings",
                c => new
                    {
                        StoredBottleID = c.Int(nullable: false),
                        TastedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ImageID = c.Guid(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.StoredBottleID);
            
            DropForeignKey("dbo.Openings", "StoredBottleID", "dbo.StoredBottles");
            DropIndex("dbo.Openings", new[] { "StoredBottleID" });
            DropTable("dbo.Openings");
            CreateIndex("dbo.Tastings", "StoredBottleID");
            AddForeignKey("dbo.Tastings", "StoredBottleID", "dbo.StoredBottles", "ID");
        }
    }
}

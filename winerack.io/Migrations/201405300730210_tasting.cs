namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tasting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tastings",
                c => new
                    {
                        StoredBottleID = c.Int(nullable: false),
                        TastedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.StoredBottleID)
                .ForeignKey("dbo.StoredBottles", t => t.StoredBottleID)
                .Index(t => t.StoredBottleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tastings", "StoredBottleID", "dbo.StoredBottles");
            DropIndex("dbo.Tastings", new[] { "StoredBottleID" });
            DropTable("dbo.Tastings");
        }
    }
}

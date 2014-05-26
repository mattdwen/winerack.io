namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bottle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bottles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WineID = c.Int(nullable: false),
                        OwnerID = c.String(nullable: false, maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerID, cascadeDelete: true)
                .ForeignKey("dbo.Wines", t => t.WineID, cascadeDelete: true)
                .Index(t => t.WineID)
                .Index(t => t.OwnerID);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BottleID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        PurchasedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PurchasePrice = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Bottles", t => t.BottleID, cascadeDelete: true)
                .Index(t => t.BottleID);
            
            AlterColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purchases", "BottleID", "dbo.Bottles");
            DropForeignKey("dbo.Bottles", "WineID", "dbo.Wines");
            DropForeignKey("dbo.Bottles", "OwnerID", "dbo.AspNetUsers");
            DropIndex("dbo.Purchases", new[] { "BottleID" });
            DropIndex("dbo.Bottles", new[] { "OwnerID" });
            DropIndex("dbo.Bottles", new[] { "WineID" });
            AlterColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime());
            DropTable("dbo.Purchases");
            DropTable("dbo.Bottles");
        }
    }
}

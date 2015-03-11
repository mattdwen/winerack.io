namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class purchasestoredrelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StoredBottles", "BottleID", "dbo.Bottles");
            DropIndex("dbo.StoredBottles", new[] { "BottleID" });
            RenameColumn(table: "dbo.StoredBottles", name: "BottleID", newName: "Bottle_ID");
            AddColumn("dbo.StoredBottles", "PurchaseID", c => c.Int(nullable: false));
            AlterColumn("dbo.StoredBottles", "Bottle_ID", c => c.Int());
            CreateIndex("dbo.StoredBottles", "PurchaseID");
            CreateIndex("dbo.StoredBottles", "Bottle_ID");
            AddForeignKey("dbo.StoredBottles", "PurchaseID", "dbo.Purchases", "ID", cascadeDelete: true);
            AddForeignKey("dbo.StoredBottles", "Bottle_ID", "dbo.Bottles", "ID");
            DropColumn("dbo.Purchases", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Purchases", "Quantity", c => c.Int(nullable: false));
            DropForeignKey("dbo.StoredBottles", "Bottle_ID", "dbo.Bottles");
            DropForeignKey("dbo.StoredBottles", "PurchaseID", "dbo.Purchases");
            DropIndex("dbo.StoredBottles", new[] { "Bottle_ID" });
            DropIndex("dbo.StoredBottles", new[] { "PurchaseID" });
            AlterColumn("dbo.StoredBottles", "Bottle_ID", c => c.Int(nullable: false));
            DropColumn("dbo.StoredBottles", "PurchaseID");
            RenameColumn(table: "dbo.StoredBottles", name: "Bottle_ID", newName: "BottleID");
            CreateIndex("dbo.StoredBottles", "BottleID");
            AddForeignKey("dbo.StoredBottles", "BottleID", "dbo.Bottles", "ID", cascadeDelete: true);
        }
    }
}

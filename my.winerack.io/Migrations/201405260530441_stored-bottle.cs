namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storedbottle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoredBottles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BottleID = c.Int(nullable: false),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Bottles", t => t.BottleID, cascadeDelete: true)
                .Index(t => t.BottleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoredBottles", "BottleID", "dbo.Bottles");
            DropIndex("dbo.StoredBottles", new[] { "BottleID" });
            DropTable("dbo.StoredBottles");
        }
    }
}

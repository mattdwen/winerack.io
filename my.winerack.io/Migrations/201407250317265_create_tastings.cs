namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_tastings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tastings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        WineID = c.Int(nullable: false),
                        ImageID = c.Guid(),
                        TastedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .ForeignKey("dbo.Wines", t => t.WineID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.WineID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tastings", "WineID", "dbo.Wines");
            DropForeignKey("dbo.Tastings", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Tastings", new[] { "WineID" });
            DropIndex("dbo.Tastings", new[] { "UserID" });
            DropTable("dbo.Tastings");
        }
    }
}

namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactor_activities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActivityEvents", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.ActivityEvents", new[] { "UserID" });
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OccuredOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ActorID = c.String(nullable: false, maxLength: 128),
                        ObjectID = c.Int(nullable: false),
                        WineID = c.Int(),
                        Verb = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ActorID, cascadeDelete: true)
                .ForeignKey("dbo.Wines", t => t.WineID)
                .Index(t => t.ActorID)
                .Index(t => t.WineID);
            
            CreateTable(
                "dbo.ActivityNotifications",
                c => new
                    {
                        ActivityID = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ActivityID, t.UserID })
                .ForeignKey("dbo.Activities", t => t.ActivityID, cascadeDelete: true)
                .Index(t => t.ActivityID);
            
            DropTable("dbo.ActivityEvents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ActivityEvents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(nullable: false, maxLength: 128),
                        OccuredOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Verb = c.Int(nullable: false),
                        Noun = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.Activities", "WineID", "dbo.Wines");
            DropForeignKey("dbo.ActivityNotifications", "ActivityID", "dbo.Activities");
            DropForeignKey("dbo.Activities", "ActorID", "dbo.AspNetUsers");
            DropIndex("dbo.ActivityNotifications", new[] { "ActivityID" });
            DropIndex("dbo.Activities", new[] { "WineID" });
            DropIndex("dbo.Activities", new[] { "ActorID" });
            DropTable("dbo.ActivityNotifications");
            DropTable("dbo.Activities");
            CreateIndex("dbo.ActivityEvents", "UserID");
            AddForeignKey("dbo.ActivityEvents", "UserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}

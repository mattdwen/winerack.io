namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_activityevents_table : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityEvents", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.ActivityEvents", new[] { "UserID" });
            DropTable("dbo.ActivityEvents");
        }
    }
}

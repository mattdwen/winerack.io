namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_friends : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FolloweeID = c.String(nullable: false, maxLength: 128),
                        FollowerID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.FolloweeID)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerID)
                .Index(t => t.FolloweeID)
                .Index(t => t.FollowerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friends", "FollowerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friends", "FolloweeID", "dbo.AspNetUsers");
            DropIndex("dbo.Friends", new[] { "FollowerID" });
            DropIndex("dbo.Friends", new[] { "FolloweeID" });
            DropTable("dbo.Friends");
        }
    }
}

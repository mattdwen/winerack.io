namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_tagged_users : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaggedUsers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ParentID = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                        ActivityVerb = c.Int(nullable: false),
                        UserType = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaggedUsers", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.TaggedUsers", new[] { "UserID" });
            DropTable("dbo.TaggedUsers");
        }
    }
}

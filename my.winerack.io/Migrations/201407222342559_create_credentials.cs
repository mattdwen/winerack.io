namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_credentials : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Credentials",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        CredentialType = c.Int(nullable: false),
                        Key = c.String(),
                        Secret = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Credentials", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Credentials", new[] { "UserID" });
            DropTable("dbo.Credentials");
        }
    }
}

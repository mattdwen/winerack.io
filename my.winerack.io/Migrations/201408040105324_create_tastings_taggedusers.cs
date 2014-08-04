namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_tastings_taggedusers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTastings",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Tasting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Tasting_ID })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tastings", t => t.Tasting_ID, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Tasting_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTastings", "Tasting_ID", "dbo.Tastings");
            DropForeignKey("dbo.UserTastings", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserTastings", new[] { "Tasting_ID" });
            DropIndex("dbo.UserTastings", new[] { "User_Id" });
            DropTable("dbo.UserTastings");
        }
    }
}

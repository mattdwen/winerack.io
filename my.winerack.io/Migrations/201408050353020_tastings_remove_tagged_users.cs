namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tastings_remove_tagged_users : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserTastings", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserTastings", "Tasting_ID", "dbo.Tastings");
            DropIndex("dbo.UserTastings", new[] { "User_Id" });
            DropIndex("dbo.UserTastings", new[] { "Tasting_ID" });
            DropTable("dbo.UserTastings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserTastings",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Tasting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Tasting_ID });
            
            CreateIndex("dbo.UserTastings", "Tasting_ID");
            CreateIndex("dbo.UserTastings", "User_Id");
            AddForeignKey("dbo.UserTastings", "Tasting_ID", "dbo.Tastings", "ID", cascadeDelete: true);
            AddForeignKey("dbo.UserTastings", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}

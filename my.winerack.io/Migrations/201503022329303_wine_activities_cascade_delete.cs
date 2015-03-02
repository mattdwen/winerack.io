namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wine_activities_cascade_delete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Activities", "WineID", "dbo.Wines");
            DropIndex("dbo.Activities", new[] { "WineID" });
            AddColumn("dbo.Activities", "Wine_ID", c => c.Int());
            AlterColumn("dbo.Activities", "WineID", c => c.Int(nullable: false));
            CreateIndex("dbo.Activities", "WineID");
            CreateIndex("dbo.Activities", "Wine_ID");
            AddForeignKey("dbo.Activities", "Wine_ID", "dbo.Wines", "ID");
            AddForeignKey("dbo.Activities", "WineID", "dbo.Wines", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "WineID", "dbo.Wines");
            DropForeignKey("dbo.Activities", "Wine_ID", "dbo.Wines");
            DropIndex("dbo.Activities", new[] { "Wine_ID" });
            DropIndex("dbo.Activities", new[] { "WineID" });
            AlterColumn("dbo.Activities", "WineID", c => c.Int());
            DropColumn("dbo.Activities", "Wine_ID");
            CreateIndex("dbo.Activities", "WineID");
            AddForeignKey("dbo.Activities", "WineID", "dbo.Wines", "ID");
        }
    }
}

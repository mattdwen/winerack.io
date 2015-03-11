namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class winestyles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Styles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.WineStyles",
                c => new
                    {
                        Wine_ID = c.Int(nullable: false),
                        Style_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Wine_ID, t.Style_ID })
                .ForeignKey("dbo.Wines", t => t.Wine_ID, cascadeDelete: true)
                .ForeignKey("dbo.Styles", t => t.Style_ID, cascadeDelete: true)
                .Index(t => t.Wine_ID)
                .Index(t => t.Style_ID);
            
            DropColumn("dbo.Varietals", "Style");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Varietals", "Style", c => c.Int(nullable: false));
            DropForeignKey("dbo.WineStyles", "Style_ID", "dbo.Styles");
            DropForeignKey("dbo.WineStyles", "Wine_ID", "dbo.Wines");
            DropIndex("dbo.WineStyles", new[] { "Style_ID" });
            DropIndex("dbo.WineStyles", new[] { "Wine_ID" });
            DropTable("dbo.WineStyles");
            DropTable("dbo.Styles");
        }
    }
}

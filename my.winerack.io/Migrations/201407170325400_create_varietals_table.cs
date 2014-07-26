namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_varietals_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Varietals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Style = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Wines", "VarietalID", c => c.Int(nullable: false));
            CreateIndex("dbo.Wines", "VarietalID");
            AddForeignKey("dbo.Wines", "VarietalID", "dbo.Varietals", "ID", cascadeDelete: true);
            DropColumn("dbo.Wines", "Varietal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Wines", "Varietal", c => c.String(nullable: false));
            DropForeignKey("dbo.Wines", "VarietalID", "dbo.Varietals");
            DropIndex("dbo.Wines", new[] { "VarietalID" });
            DropColumn("dbo.Wines", "VarietalID");
            DropTable("dbo.Varietals");
        }
    }
}

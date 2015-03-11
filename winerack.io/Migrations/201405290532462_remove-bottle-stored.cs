namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removebottlestored : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StoredBottles", "Bottle_ID", "dbo.Bottles");
            DropIndex("dbo.StoredBottles", new[] { "Bottle_ID" });
            DropColumn("dbo.StoredBottles", "Bottle_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StoredBottles", "Bottle_ID", c => c.Int());
            CreateIndex("dbo.StoredBottles", "Bottle_ID");
            AddForeignKey("dbo.StoredBottles", "Bottle_ID", "dbo.Bottles", "ID");
        }
    }
}

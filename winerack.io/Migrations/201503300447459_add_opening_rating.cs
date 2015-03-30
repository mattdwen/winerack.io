namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_opening_rating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Openings", "Rating", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Openings", "Rating");
        }
    }
}

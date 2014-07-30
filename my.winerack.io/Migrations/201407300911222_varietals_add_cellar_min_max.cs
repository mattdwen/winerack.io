namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class varietals_add_cellar_min_max : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Varietals", "CellarMin", c => c.Int());
            AddColumn("dbo.Varietals", "CellarMax", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Varietals", "CellarMax");
            DropColumn("dbo.Varietals", "CellarMin");
        }
    }
}

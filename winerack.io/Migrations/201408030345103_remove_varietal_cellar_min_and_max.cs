namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_varietal_cellar_min_and_max : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Varietals", "CellarMin");
            DropColumn("dbo.Varietals", "CellarMax");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Varietals", "CellarMax", c => c.Int());
            AddColumn("dbo.Varietals", "CellarMin", c => c.Int());
        }
    }
}

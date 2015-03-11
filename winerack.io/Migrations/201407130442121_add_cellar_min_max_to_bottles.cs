namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_cellar_min_max_to_bottles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bottles", "CellarMin", c => c.Int(nullable: false));
            AddColumn("dbo.Bottles", "CellarMax", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bottles", "CellarMax");
            DropColumn("dbo.Bottles", "CellarMin");
        }
    }
}

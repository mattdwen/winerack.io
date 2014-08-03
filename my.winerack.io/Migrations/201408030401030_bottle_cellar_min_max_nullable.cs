namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bottle_cellar_min_max_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bottles", "CellarMin", c => c.Int());
            AlterColumn("dbo.Bottles", "CellarMax", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bottles", "CellarMax", c => c.Int(nullable: false));
            AlterColumn("dbo.Bottles", "CellarMin", c => c.Int(nullable: false));
        }
    }
}

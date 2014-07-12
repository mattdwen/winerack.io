namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_imageid_to_tastings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tastings", "ImageID", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tastings", "ImageID");
        }
    }
}

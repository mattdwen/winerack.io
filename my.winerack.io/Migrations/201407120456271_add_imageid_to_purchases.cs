namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_imageid_to_purchases : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchases", "ImageID", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purchases", "ImageID");
        }
    }
}

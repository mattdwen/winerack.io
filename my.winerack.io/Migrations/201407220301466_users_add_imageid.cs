namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class users_add_imageid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ImageID", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ImageID");
        }
    }
}

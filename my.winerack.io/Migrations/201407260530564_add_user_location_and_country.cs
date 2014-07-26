namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_user_location_and_country : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Location", c => c.String(maxLength: 255));
            AddColumn("dbo.AspNetUsers", "Country", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Country");
            DropColumn("dbo.AspNetUsers", "Location");
        }
    }
}

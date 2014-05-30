namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usercreateddate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreatedOn", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CreatedOn");
        }
    }
}

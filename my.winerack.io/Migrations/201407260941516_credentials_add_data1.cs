namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class credentials_add_data1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Credentials", "Data1", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Credentials", "Data1");
        }
    }
}

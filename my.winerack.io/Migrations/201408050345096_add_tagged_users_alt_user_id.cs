namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_tagged_users_alt_user_id : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaggedUsers", "AltUserID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaggedUsers", "AltUserID");
        }
    }
}

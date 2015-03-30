namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_purchase_isgift : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchases", "IsGift", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purchases", "IsGift");
        }
    }
}

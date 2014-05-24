namespace my.winerack.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class purchasenullableprice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchases", "PurchasePrice", c => c.Decimal(storeType: "money"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Purchases", "PurchasePrice", c => c.Decimal(nullable: false, storeType: "money"));
        }
    }
}

namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullpurchasedate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Purchases", "PurchasedOn", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Purchases", "PurchasedOn", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}

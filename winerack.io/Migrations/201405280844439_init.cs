namespace winerack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bottles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WineID = c.Int(nullable: false),
                        OwnerID = c.String(nullable: false, maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerID, cascadeDelete: true)
                .ForeignKey("dbo.Wines", t => t.WineID, cascadeDelete: true)
                .Index(t => t.WineID)
                .Index(t => t.OwnerID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(maxLength: 255),
                        LastName = c.String(maxLength: 255),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BottleID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        PurchasedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        PurchasePrice = c.Decimal(storeType: "money"),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Bottles", t => t.BottleID, cascadeDelete: true)
                .Index(t => t.BottleID);
            
            CreateTable(
                "dbo.StoredBottles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BottleID = c.Int(nullable: false),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Bottles", t => t.BottleID, cascadeDelete: true)
                .Index(t => t.BottleID);
            
            CreateTable(
                "dbo.Wines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Varietal = c.String(nullable: false),
                        Vintage = c.Int(),
                        RegionID = c.Int(nullable: false),
                        VineyardID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Regions", t => t.RegionID, cascadeDelete: true)
                .ForeignKey("dbo.Vineyards", t => t.VineyardID, cascadeDelete: true)
                .Index(t => t.RegionID)
                .Index(t => t.VineyardID);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Country = c.String(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Vineyards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Bottles", "WineID", "dbo.Wines");
            DropForeignKey("dbo.Wines", "VineyardID", "dbo.Vineyards");
            DropForeignKey("dbo.Wines", "RegionID", "dbo.Regions");
            DropForeignKey("dbo.StoredBottles", "BottleID", "dbo.Bottles");
            DropForeignKey("dbo.Purchases", "BottleID", "dbo.Bottles");
            DropForeignKey("dbo.Bottles", "OwnerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Wines", new[] { "VineyardID" });
            DropIndex("dbo.Wines", new[] { "RegionID" });
            DropIndex("dbo.StoredBottles", new[] { "BottleID" });
            DropIndex("dbo.Purchases", new[] { "BottleID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Bottles", new[] { "OwnerID" });
            DropIndex("dbo.Bottles", new[] { "WineID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Vineyards");
            DropTable("dbo.Regions");
            DropTable("dbo.Wines");
            DropTable("dbo.StoredBottles");
            DropTable("dbo.Purchases");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Bottles");
        }
    }
}

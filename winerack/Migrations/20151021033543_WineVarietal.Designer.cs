using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using winerack.Models;

namespace winerack.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20151021033543_WineVarietal")]
    partial class WineVarietal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta8-15964")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .Annotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .Annotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.Index("NormalizedName")
                        .Annotation("Relational:Name", "RoleNameIndex");

                    b.Annotation("Relational:TableName", "AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId");

                    b.HasKey("Id");

                    b.Annotation("Relational:TableName", "AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.Annotation("Relational:TableName", "AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.Annotation("Relational:TableName", "AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.Annotation("Relational:TableName", "AspNetUserRoles");
                });

            modelBuilder.Entity("winerack.Models.Activity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActorID")
                        .IsRequired();

                    b.Property<int>("ObjectID");

                    b.Property<DateTime>("OccuredOn");

                    b.Property<int>("Verb");

                    b.Property<int?>("WineID");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.ActivityNotification", b =>
                {
                    b.Property<int>("ActivityID");

                    b.Property<string>("UserID");

                    b.HasKey("ActivityID", "UserID");
                });

            modelBuilder.Entity("winerack.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country")
                        .Annotation("MaxLength", 255);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email")
                        .Annotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .Annotation("MaxLength", 255);

                    b.Property<Guid?>("ImageID");

                    b.Property<string>("LastName")
                        .Annotation("MaxLength", 255);

                    b.Property<string>("Location")
                        .Annotation("MaxLength", 255);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .Annotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .Annotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .Annotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.Index("NormalizedEmail")
                        .Annotation("Relational:Name", "EmailIndex");

                    b.Index("NormalizedUserName")
                        .Annotation("Relational:Name", "UserNameIndex");

                    b.Annotation("Relational:TableName", "AspNetUsers");
                });

            modelBuilder.Entity("winerack.Models.Bottle", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CellarMax");

                    b.Property<int?>("CellarMin");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("OwnerID")
                        .IsRequired();

                    b.Property<int>("WineID");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.Credentials", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CredentialType");

                    b.Property<string>("Data1");

                    b.Property<string>("Key");

                    b.Property<string>("Secret");

                    b.Property<string>("UserID");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.Friend", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("FolloweeID")
                        .IsRequired();

                    b.Property<string>("FollowerID")
                        .IsRequired();

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.Opening", b =>
                {
                    b.Property<int>("StoredBottleID");

                    b.Property<Guid?>("ImageID");

                    b.Property<string>("Notes");

                    b.Property<DateTime?>("OpenedOn");

                    b.Property<int?>("Rating");

                    b.HasKey("StoredBottleID");
                });

            modelBuilder.Entity("winerack.Models.Purchase", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BottleID");

                    b.Property<Guid?>("ImageID");

                    b.Property<bool>("IsGift");

                    b.Property<string>("Notes");

                    b.Property<decimal?>("PurchasePrice")
                        .Annotation("Relational:ColumnType", "Money");

                    b.Property<DateTime?>("PurchasedOn");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.Region", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Country")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.StoredBottle", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Location");

                    b.Property<int>("PurchaseID");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.Style", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.Tasting", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ImageID");

                    b.Property<string>("Notes");

                    b.Property<DateTime>("TastedOn");

                    b.Property<string>("UserID");

                    b.Property<int>("WineID");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.Varietal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.Vineyard", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("RegionID");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.Wine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("RegionID");

                    b.Property<int>("VineyardID");

                    b.Property<int?>("Vintage");

                    b.HasKey("ID");
                });

            modelBuilder.Entity("winerack.Models.WineVarietals", b =>
                {
                    b.Property<int>("WineId");

                    b.Property<int>("VarietalId");

                    b.HasKey("WineId", "VarietalId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .ForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("winerack.Models.ApplicationUser")
                        .WithMany()
                        .ForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("winerack.Models.ApplicationUser")
                        .WithMany()
                        .ForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .ForeignKey("RoleId");

                    b.HasOne("winerack.Models.ApplicationUser")
                        .WithMany()
                        .ForeignKey("UserId");
                });

            modelBuilder.Entity("winerack.Models.Activity", b =>
                {
                    b.HasOne("winerack.Models.ApplicationUser")
                        .WithMany()
                        .ForeignKey("ActorID");

                    b.HasOne("winerack.Models.Wine")
                        .WithMany()
                        .ForeignKey("WineID");
                });

            modelBuilder.Entity("winerack.Models.ActivityNotification", b =>
                {
                    b.HasOne("winerack.Models.Activity")
                        .WithMany()
                        .ForeignKey("ActivityID");
                });

            modelBuilder.Entity("winerack.Models.Bottle", b =>
                {
                    b.HasOne("winerack.Models.ApplicationUser")
                        .WithMany()
                        .ForeignKey("OwnerID");

                    b.HasOne("winerack.Models.Wine")
                        .WithMany()
                        .ForeignKey("WineID");
                });

            modelBuilder.Entity("winerack.Models.Credentials", b =>
                {
                    b.HasOne("winerack.Models.ApplicationUser")
                        .WithMany()
                        .ForeignKey("UserID");
                });

            modelBuilder.Entity("winerack.Models.Opening", b =>
                {
                    b.HasOne("winerack.Models.StoredBottle")
                        .WithOne()
                        .ForeignKey("winerack.Models.Opening", "StoredBottleID");
                });

            modelBuilder.Entity("winerack.Models.Purchase", b =>
                {
                    b.HasOne("winerack.Models.Bottle")
                        .WithMany()
                        .ForeignKey("BottleID");
                });

            modelBuilder.Entity("winerack.Models.StoredBottle", b =>
                {
                    b.HasOne("winerack.Models.Purchase")
                        .WithMany()
                        .ForeignKey("PurchaseID");
                });

            modelBuilder.Entity("winerack.Models.Tasting", b =>
                {
                    b.HasOne("winerack.Models.ApplicationUser")
                        .WithMany()
                        .ForeignKey("UserID");

                    b.HasOne("winerack.Models.Wine")
                        .WithMany()
                        .ForeignKey("WineID");
                });

            modelBuilder.Entity("winerack.Models.Vineyard", b =>
                {
                    b.HasOne("winerack.Models.Region")
                        .WithMany()
                        .ForeignKey("RegionID");
                });

            modelBuilder.Entity("winerack.Models.Wine", b =>
                {
                    b.HasOne("winerack.Models.Region")
                        .WithMany()
                        .ForeignKey("RegionID");

                    b.HasOne("winerack.Models.Vineyard")
                        .WithMany()
                        .ForeignKey("VineyardID");
                });

            modelBuilder.Entity("winerack.Models.WineVarietals", b =>
                {
                    b.HasOne("winerack.Models.Varietal")
                        .WithMany()
                        .ForeignKey("VarietalId");

                    b.HasOne("winerack.Models.Wine")
                        .WithMany()
                        .ForeignKey("WineId");
                });
        }
    }
}

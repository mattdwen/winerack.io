using System;
using Microsoft.Data.Entity.Migrations;

namespace winerack.Migrations
{
  public partial class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>("ConcurrencyStamp", "AspNetUsers", nullable: true);
      migrationBuilder.AddColumn<DateTimeOffset>("LockoutEnd", "AspNetUsers", nullable: true);
      migrationBuilder.AddColumn<string>("NormalizedEmail", "AspNetUsers", nullable: true);
      migrationBuilder.AddColumn<string>("NormalizedUserName", "AspNetUsers", nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
  }
}
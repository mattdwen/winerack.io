using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace winerack.Migrations
{
    public partial class WineVarietal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WineVarietals",
                columns: table => new
                {
                    WineId = table.Column<int>(nullable: false),
                    VarietalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineVarietals", x => new { x.WineId, x.VarietalId });
                    table.ForeignKey(
                        name: "FK_WineVarietals_Varietal_VarietalId",
                        column: x => x.VarietalId,
                        principalTable: "Varietal",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_WineVarietals_Wine_WineId",
                        column: x => x.WineId,
                        principalTable: "Wine",
                        principalColumn: "ID");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("WineVarietals");
        }
    }
}

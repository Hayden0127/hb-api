using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class TableCreation_DataSeeding_PriceVaries_Unit_ProductType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceVaries",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceVaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductType",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "cpms",
                table: "PriceVaries",
                columns: new[] { "Id", "Name", "CreatedOn", "IsActive" },
                values: new object[,]
                {
                    { 1, "FOC", DateTime.UtcNow, true },
                    { 2, "$Per Unit", DateTime.UtcNow, true },
                    { 3, "$Per Blocks", DateTime.UtcNow, true }
                });

            migrationBuilder.InsertData(
                schema: "cpms",
                table: "ProductType",
                columns: new[] { "Id", "Name", "CreatedOn", "IsActive" },
                values: new object[,]
                {
                    { 1, "AC", DateTime.UtcNow, true },
                    { 2, "DC", DateTime.UtcNow, true },
                    { 3, "AC/DC", DateTime.UtcNow, true }
                });

            migrationBuilder.InsertData(
                schema: "cpms",
                table: "Unit",
                columns: new[] { "Id", "Name", "CreatedOn", "IsActive" },
                values: new object[,]
                {
                    { 1, "FOC", DateTime.UtcNow, true },
                    { 2, "kWh", DateTime.UtcNow, true },
                    { 3, "Minutes", DateTime.UtcNow, true },
                    { 4, "Hours", DateTime.UtcNow, true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceVaries",
                schema: "cpms");

            migrationBuilder.DropTable(
                name: "ProductType",
                schema: "cpms");

            migrationBuilder.DropTable(
                name: "Unit",
                schema: "cpms");
        }
    }
}

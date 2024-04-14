using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class TableCreation_MeterValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeterValue",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CPDetailsId = table.Column<int>(type: "int", nullable: false),
                    ConnectorId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    UniqueId = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentMeterValue = table.Column<int>(type: "int", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Measurand = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterValue", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterValue",
                schema: "cpms");
        }
    }
}

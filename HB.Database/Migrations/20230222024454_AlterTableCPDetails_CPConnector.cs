using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AlterTableCPDetails_CPConnector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceID",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "cpms",
                table: "CPDetails",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ConnectorId",
                schema: "cpms",
                table: "CPConnector",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                schema: "cpms",
                table: "CPConnector",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "cpms",
                table: "CPConnector",
                type: "nvarchar(20)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectorId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropColumn(
                name: "ProductType",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "cpms",
                table: "CPDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceID",
                schema: "cpms",
                table: "CPConnector",
                type: "nvarchar(50)",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AlterTableColumnsCPDetailsId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectorId",
                

                table: "CPDetails");

            migrationBuilder.RenameColumn(
                name: "CPSiteDetailsId",
                schema: "cpms",
                table: "CPTransaction",
                newName: "CPDetailsId");

            migrationBuilder.RenameColumn(
                name: "CPSiteDetailsId",
                schema: "cpms",
                table: "CPBreakdownError",
                newName: "CPDetailsId");

            migrationBuilder.AddColumn<string>(
                name: "IdTag",
                schema: "cpms",
                table: "CPDetails",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConnectorId",
                schema: "cpms",
                table: "CPConnector",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdTag",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "ConnectorId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.RenameColumn(
                name: "CPDetailsId",
                schema: "cpms",
                table: "CPTransaction",
                newName: "CPSiteDetailsId");

            migrationBuilder.RenameColumn(
                name: "CPDetailsId",
                schema: "cpms",
                table: "CPBreakdownError",
                newName: "CPSiteDetailsId");

            migrationBuilder.AddColumn<int>(
                name: "ConnectorId",
                schema: "cpms",
                table: "CPDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

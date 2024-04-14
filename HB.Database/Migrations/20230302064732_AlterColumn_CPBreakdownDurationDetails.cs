using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AlterColumn_CPBreakdownDurationDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPSiteDetailsId",
                schema: "cpms",
                table: "CPBreakdownDurationDetails");

            migrationBuilder.DropColumn(
                name: "ConnectorId",
                schema: "cpms",
                table: "CPBreakdownDurationDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CPSiteDetailsId",
                schema: "cpms",
                table: "CPBreakdownDurationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConnectorId",
                schema: "cpms",
                table: "CPBreakdownDurationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

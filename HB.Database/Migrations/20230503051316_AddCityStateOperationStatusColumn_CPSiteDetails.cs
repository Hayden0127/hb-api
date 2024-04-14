using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AddCityStateOperationStatusColumn_CPSiteDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "cpms",
                table: "CPSiteDetails",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperationalStatus",
                schema: "cpms",
                table: "CPSiteDetails",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "cpms",
                table: "CPSiteDetails",
                type: "nvarchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "cpms",
                table: "CPSiteDetails");

            migrationBuilder.DropColumn(
                name: "OperationalStatus",
                schema: "cpms",
                table: "CPSiteDetails");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "cpms",
                table: "CPSiteDetails");
        }
    }
}

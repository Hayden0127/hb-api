using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AddColumnSMARTSD_AddColumnIsOnlineStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SmartSDSiteId",
                schema: "cpms",
                table: "CPSiteDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                schema: "cpms",
                table: "CPDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmartSDSiteId",
                schema: "cpms",
                table: "CPSiteDetails");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                schema: "cpms",
                table: "CPDetails");
        }
    }
}

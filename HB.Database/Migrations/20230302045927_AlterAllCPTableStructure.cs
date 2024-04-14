using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AlterAllCPTableStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CPDetails_ProfileDetails_ProfileDetailsId",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropTable(
                name: "ProfileDetails",
                schema: "cpms");

            migrationBuilder.DropIndex(
                name: "IX_CPDetails_ProfileDetailsId",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "ConnectorId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.RenameColumn(
                name: "CPDetailsId",
                schema: "cpms",
                table: "CPTransaction",
                newName: "CPSiteDetailsId");

            migrationBuilder.RenameColumn(
                name: "ProfileDetailsId",
                schema: "cpms",
                table: "CPDetails",
                newName: "ConnectorId");

            migrationBuilder.RenameColumn(
                name: "ConnectorName",
                schema: "cpms",
                table: "CPConnector",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CPDetailsId",
                schema: "cpms",
                table: "CPBreakdownError",
                newName: "CPSiteDetailsId");

            migrationBuilder.RenameColumn(
                name: "CPDetailsId",
                schema: "cpms",
                table: "CPBreakdownDurationDetails",
                newName: "CPSiteDetailsId");

            migrationBuilder.AddColumn<int>(
                name: "CPSiteDetailsId",
                schema: "cpms",
                table: "CPDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "cpms",
                table: "CPDetails",
                type: "nvarchar(15)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CPSiteDetails",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    PersonInCharge = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    OfficeNo = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    MaintenanceProgram = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPSiteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CPSiteDetails_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalSchema: "cpms",
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CPDetails_CPSiteDetailsId",
                schema: "cpms",
                table: "CPDetails",
                column: "CPSiteDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_CPSiteDetails_UserAccountId",
                schema: "cpms",
                table: "CPSiteDetails",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_CPDetails_CPSiteDetails_CPSiteDetailsId",
                schema: "cpms",
                table: "CPDetails",
                column: "CPSiteDetailsId",
                principalSchema: "cpms",
                principalTable: "CPSiteDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CPDetails_CPSiteDetails_CPSiteDetailsId",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropTable(
                name: "CPSiteDetails",
                schema: "cpms");

            migrationBuilder.DropIndex(
                name: "IX_CPDetails_CPSiteDetailsId",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "CPSiteDetailsId",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.RenameColumn(
                name: "CPSiteDetailsId",
                schema: "cpms",
                table: "CPTransaction",
                newName: "CPDetailsId");

            migrationBuilder.RenameColumn(
                name: "ConnectorId",
                schema: "cpms",
                table: "CPDetails",
                newName: "ProfileDetailsId");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "cpms",
                table: "CPConnector",
                newName: "ConnectorName");

            migrationBuilder.RenameColumn(
                name: "CPSiteDetailsId",
                schema: "cpms",
                table: "CPBreakdownError",
                newName: "CPDetailsId");

            migrationBuilder.RenameColumn(
                name: "CPSiteDetailsId",
                schema: "cpms",
                table: "CPBreakdownDurationDetails",
                newName: "CPDetailsId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "cpms",
                table: "CPDetails",
                type: "nvarchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "cpms",
                table: "CPDetails",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                schema: "cpms",
                table: "CPDetails",
                type: "decimal(18,10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                schema: "cpms",
                table: "CPDetails",
                type: "decimal(18,10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ConnectorId",
                schema: "cpms",
                table: "CPConnector",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "cpms",
                table: "CPConnector",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfileDetails",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsOtherLocation = table.Column<bool>(type: "bit", nullable: false),
                    MaintenanceProgram = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    OfficeNo = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CPDetails_ProfileDetailsId",
                schema: "cpms",
                table: "CPDetails",
                column: "ProfileDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CPDetails_ProfileDetails_ProfileDetailsId",
                schema: "cpms",
                table: "CPDetails",
                column: "ProfileDetailsId",
                principalSchema: "cpms",
                principalTable: "ProfileDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

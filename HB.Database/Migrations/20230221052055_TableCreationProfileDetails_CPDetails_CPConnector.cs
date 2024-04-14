using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class TableCreationProfileDetails_CPDetails_CPConnector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileDetails",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    OfficeNo = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    IsOtherLocation = table.Column<bool>(type: "bit", nullable: false),
                    MaintenanceProgram = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPDetails",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileDetailsId = table.Column<int>(type: "int", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CPDetails_ProfileDetails_ProfileDetailsId",
                        column: x => x.ProfileDetailsId,
                        principalSchema: "cpms",
                        principalTable: "ProfileDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CPConnector",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CPDetailsId = table.Column<int>(type: "int", nullable: false),
                    DeviceID = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ConnectorName = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPConnector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CPConnector_CPDetails_CPDetailsId",
                        column: x => x.CPDetailsId,
                        principalSchema: "cpms",
                        principalTable: "CPDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CPConnector_CPDetailsId",
                schema: "cpms",
                table: "CPConnector",
                column: "CPDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_CPDetails_ProfileDetailsId",
                schema: "cpms",
                table: "CPDetails",
                column: "ProfileDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CPConnector",
                schema: "cpms");

            migrationBuilder.DropTable(
                name: "CPDetails",
                schema: "cpms");

            migrationBuilder.DropTable(
                name: "ProfileDetails",
                schema: "cpms");

            migrationBuilder.CreateTable(
                name: "CMSRequestLog",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RequestType = table.Column<byte>(type: "tinyint", nullable: false),
                    ResponseCode = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMSRequestLog", x => x.Id);
                });
        }
    }
}

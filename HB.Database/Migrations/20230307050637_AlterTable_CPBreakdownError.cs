using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AlterTable_CPBreakdownError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                

                table: "CPBreakdownError",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Severity",
                schema: "cpms",
                table: "CPBreakdownError",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                schema: "cpms",
                table: "CPBreakdownError",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IncidentNo",
                schema: "cpms",
                table: "CPBreakdownError",
                type: "nvarchar(40)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncidentId",
                schema: "cpms",
                table: "CPBreakdownError");

            migrationBuilder.DropColumn(
                name: "IncidentNo",
                schema: "cpms",
                table: "CPBreakdownError");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "cpms",
                table: "CPBreakdownError",
                type: "nvarchar(30)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Severity",
                schema: "cpms",
                table: "CPBreakdownError",
                type: "nvarchar(30)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

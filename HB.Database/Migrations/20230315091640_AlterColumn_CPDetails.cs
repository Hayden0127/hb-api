using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AlterColumn_CPDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdTag",
                schema: "cpms",
                table: "CPDetails",
                newName: "SerialNo");

            migrationBuilder.AddColumn<DateTime>(
                name: "HeartbeatDateTime",
                schema: "cpms",
                table: "CPDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WebSocketId",
                schema: "cpms",
                table: "CPDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeartbeatDateTime",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "WebSocketId",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.RenameColumn(
                name: "SerialNo",
                schema: "cpms",
                table: "CPDetails",
                newName: "IdTag");
        }
    }
}

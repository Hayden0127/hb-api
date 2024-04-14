using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class UpdateHBTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                schema: "cpms",
                table: "PaymentDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOutDate",
                schema: "cpms",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInDate",
                schema: "cpms",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserAccountId",
                schema: "cpms",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserAccountId",
                schema: "cpms",
                table: "Booking",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_UserAccount_UserAccountId",
                schema: "cpms",
                table: "Booking",
                column: "UserAccountId",
                principalSchema: "cpms",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_UserAccount_UserAccountId",
                schema: "cpms",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_UserAccountId",
                schema: "cpms",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                schema: "cpms",
                table: "Booking");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                schema: "cpms",
                table: "PaymentDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOutDate",
                schema: "cpms",
                table: "Booking",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInDate",
                schema: "cpms",
                table: "Booking",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}

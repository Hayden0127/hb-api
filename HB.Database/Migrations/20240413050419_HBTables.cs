using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class HBTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Booking",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adult = table.Column<int>(type: "int", nullable: false),
                    Children = table.Column<int>(type: "int", nullable: false),
                    Person = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Src = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Bed = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuestDetails",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestDetails_Booking_BookingId",
                        column: x => x.BookingId,
                        principalSchema: "cpms",
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    CVV = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    NameOnCard = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_Booking_BookingId",
                        column: x => x.BookingId,
                        principalSchema: "cpms",
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuestDetails_BookingId",
                schema: "cpms",
                table: "GuestDetails",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_BookingId",
                schema: "cpms",
                table: "PaymentDetails",
                column: "BookingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuestDetails",
                schema: "cpms");

            migrationBuilder.DropTable(
                name: "PaymentDetails",
                schema: "cpms");

            migrationBuilder.DropTable(
                name: "Booking",
                schema: "cpms");
        }
    }
}

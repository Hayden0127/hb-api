using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class TableCreationUserAccount_UserAccountAuthorizationToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccount",
                schema: "cpms",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    SaltHashPassword = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    IsTemporaryPassword = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },

                constraints: table => {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccountAuthorizationToken",
                schema: "cpms",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UserAccountAuthorizationToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccountAuthorizationToken_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalSchema: "cpms",
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountAuthorizationToken_UserAccountId",
                schema: "cpms",
                table: "UserAccountAuthorizationToken",
                column: "UserAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccountAuthorizationToken",
                schema: "cpms");

            migrationBuilder.DropTable(
                name: "UserAccount",
                schema: "cpms");
        }
    }
}

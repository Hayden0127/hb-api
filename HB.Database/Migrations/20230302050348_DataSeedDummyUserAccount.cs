using Microsoft.EntityFrameworkCore.Migrations;
using Strateq.Core.Utilities;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class DataSeedDummyUserAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var salt = SaltEncryption.generateSalt();
            var saltHashPassword = SaltEncryption.hash("!234Qwer", salt);

            migrationBuilder.InsertData(
              

              table: "UserAccount",
              columns: new[] { "Id", "FullName", "Email", "Salt", "SaltHashPassword", "IsTemporaryPassword", "CreatedOn", "IsActive" },
              values: new object[,]
              {
                    { 1, "User 1", "demo1@strateqgroup.com",salt,saltHashPassword, false, DateTime.Now, true },
                    { 2, "User 2", "demo2@strateqgroup.com",salt,saltHashPassword, false, DateTime.Now, true },
                    { 3, "User 3", "demo3@strateqgroup.com",salt,saltHashPassword, false, DateTime.Now, true },
              });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

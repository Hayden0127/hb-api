using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class DataSeedDummyRecordsUserAccountAuthorizationToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
              

              table: "UserAccountAuthorizationToken",
              columns: new[] { "Id", "UserAccountId", "RefreshToken", "CreatedOn", "IsActive" },
              values: new object[,]
              {
                    { 1, 1, string.Empty, DateTime.Now, true },
                    { 2, 2, string.Empty, DateTime.Now, true },
                    { 3, 3, string.Empty, DateTime.Now, true },
              });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

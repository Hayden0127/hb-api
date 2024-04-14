using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class DataSeedingRunningSequenceNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
              schema: "cpms",
              table: "RunningSequenceNumber",
              columns: new[] { "Id", "Type", "SequenceNumber", "CreatedOn", "IsActive" },
              values: new object[,]
              {
                   { 1, "TransactionId", 0, DateTime.UtcNow, true }
             });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

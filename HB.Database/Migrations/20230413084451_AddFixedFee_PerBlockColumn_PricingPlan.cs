using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AddFixedFee_PerBlockColumn_PricingPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FixedFee",
                

                table: "PricingPlan",
                type: "decimal(10,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PerBlock",
                schema: "cpms",
                table: "PricingPlan",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedFee",
                schema: "cpms",
                table: "PricingPlan");

            migrationBuilder.DropColumn(
                name: "PerBlock",
                schema: "cpms",
                table: "PricingPlan");
        }
    }
}

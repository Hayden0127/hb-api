using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AlterColumnPerBlockToPricingPlanTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerBlock",
                

                table: "PricingPlanType");

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
                name: "PerBlock",
                schema: "cpms",
                table: "PricingPlan");

            migrationBuilder.AddColumn<int>(
                name: "PerBlock",
                schema: "cpms",
                table: "PricingPlanType",
                type: "int",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class AlterTable_CPConnector_TableCreation_PricingPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                schema: "cpms",
                table: "CPTransaction");

            migrationBuilder.DropColumn(
                name: "ProductType",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.AddColumn<int>(
                name: "ProductTypeId",
                schema: "cpms",
                table: "CPTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedOn",
                schema: "cpms",
                table: "CPConnector",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "cpms",
                table: "CPConnector",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PricingPlanId",
                schema: "cpms",
                table: "CPConnector",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductTypeId",
                schema: "cpms",
                table: "CPConnector",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PricingPlan",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    PriceVariesId = table.Column<int>(type: "int", nullable: false),
                    PriceRate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricingPlan_PriceVaries_PriceVariesId",
                        column: x => x.PriceVariesId,
                        principalSchema: "cpms",
                        principalTable: "PriceVaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PricingPlan_ProductType_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalSchema: "cpms",
                        principalTable: "ProductType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PricingPlan_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "cpms",
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CPTransaction_ProductTypeId",
                schema: "cpms",
                table: "CPTransaction",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CPConnector_PricingPlanId",
                schema: "cpms",
                table: "CPConnector",
                column: "PricingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_CPConnector_ProductTypeId",
                schema: "cpms",
                table: "CPConnector",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingPlan_PriceVariesId",
                schema: "cpms",
                table: "PricingPlan",
                column: "PriceVariesId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingPlan_ProductTypeId",
                schema: "cpms",
                table: "PricingPlan",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingPlan_UnitId",
                schema: "cpms",
                table: "PricingPlan",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_CPConnector_PricingPlan_PricingPlanId",
                schema: "cpms",
                table: "CPConnector",
                column: "PricingPlanId",
                principalSchema: "cpms",
                principalTable: "PricingPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CPConnector_ProductType_ProductTypeId",
                schema: "cpms",
                table: "CPConnector",
                column: "ProductTypeId",
                principalSchema: "cpms",
                principalTable: "ProductType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CPTransaction_ProductType_ProductTypeId",
                schema: "cpms",
                table: "CPTransaction",
                column: "ProductTypeId",
                principalSchema: "cpms",
                principalTable: "ProductType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CPConnector_PricingPlan_PricingPlanId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropForeignKey(
                name: "FK_CPConnector_ProductType_ProductTypeId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropForeignKey(
                name: "FK_CPTransaction_ProductType_ProductTypeId",
                schema: "cpms",
                table: "CPTransaction");

            migrationBuilder.DropTable(
                name: "PricingPlan",
                schema: "cpms");

            migrationBuilder.DropIndex(
                name: "IX_CPTransaction_ProductTypeId",
                schema: "cpms",
                table: "CPTransaction");

            migrationBuilder.DropIndex(
                name: "IX_CPConnector_PricingPlanId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropIndex(
                name: "IX_CPConnector_ProductTypeId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                schema: "cpms",
                table: "CPTransaction");

            migrationBuilder.DropColumn(
                name: "AssignedOn",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropColumn(
                name: "PricingPlanId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                schema: "cpms",
                table: "CPTransaction",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                schema: "cpms",
                table: "CPConnector",
                type: "nvarchar(100)",
                nullable: true);
        }
    }
}

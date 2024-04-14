using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HB.Database.Migrations
{
    public partial class TableCreationPricingPlanType_AlterTableColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CPConnector_PricingPlan_PricingPlanId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropForeignKey(
                name: "FK_PricingPlan_ProductType_ProductTypeId",
                schema: "cpms",
                table: "PricingPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_PricingPlan_Unit_UnitId",
                schema: "cpms",
                table: "PricingPlan");

            migrationBuilder.DropIndex(
                name: "IX_PricingPlan_ProductTypeId",
                schema: "cpms",
                table: "PricingPlan");

            migrationBuilder.DropIndex(
                name: "IX_CPConnector_PricingPlanId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropColumn(
                name: "PerBlock",
                schema: "cpms",
                table: "PricingPlan");

            migrationBuilder.DropColumn(
                name: "PriceRate",
                schema: "cpms",
                table: "PricingPlan");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                schema: "cpms",
                table: "PricingPlan");

            migrationBuilder.DropColumn(
                name: "AssignedOn",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.DropColumn(
                name: "PricingPlanId",
                schema: "cpms",
                table: "CPConnector");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                schema: "cpms",
                table: "PricingPlan",
                newName: "UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_PricingPlan_UnitId",
                schema: "cpms",
                table: "PricingPlan",
                newName: "IX_PricingPlan_UserAccountId");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedOn",
                schema: "cpms",
                table: "CPDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "cpms",
                table: "CPDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PricingPlanId",
                schema: "cpms",
                table: "CPDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PricingPlanType",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricingPlanId = table.Column<int>(type: "int", nullable: false),
                    PriceRate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    FixedFee = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    PerBlock = table.Column<int>(type: "int", nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingPlanType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricingPlanType_PricingPlan_PricingPlanId",
                        column: x => x.PricingPlanId,
                        principalSchema: "cpms",
                        principalTable: "PricingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PricingPlanType_ProductType_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalSchema: "cpms",
                        principalTable: "ProductType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PricingPlanType_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "cpms",
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CPDetails_PricingPlanId",
                schema: "cpms",
                table: "CPDetails",
                column: "PricingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingPlanType_PricingPlanId",
                schema: "cpms",
                table: "PricingPlanType",
                column: "PricingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingPlanType_ProductTypeId",
                schema: "cpms",
                table: "PricingPlanType",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingPlanType_UnitId",
                schema: "cpms",
                table: "PricingPlanType",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_CPDetails_PricingPlan_PricingPlanId",
                schema: "cpms",
                table: "CPDetails",
                column: "PricingPlanId",
                principalSchema: "cpms",
                principalTable: "PricingPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PricingPlan_UserAccount_UserAccountId",
                schema: "cpms",
                table: "PricingPlan",
                column: "UserAccountId",
                principalSchema: "cpms",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CPDetails_PricingPlan_PricingPlanId",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PricingPlan_UserAccount_UserAccountId",
                schema: "cpms",
                table: "PricingPlan");

            migrationBuilder.DropTable(
                name: "PricingPlanType",
                schema: "cpms");

            migrationBuilder.DropIndex(
                name: "IX_CPDetails_PricingPlanId",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "AssignedOn",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.DropColumn(
                name: "PricingPlanId",
                schema: "cpms",
                table: "CPDetails");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                schema: "cpms",
                table: "PricingPlan",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_PricingPlan_UserAccountId",
                schema: "cpms",
                table: "PricingPlan",
                newName: "IX_PricingPlan_UnitId");

            migrationBuilder.AddColumn<int>(
                name: "PerBlock",
                schema: "cpms",
                table: "PricingPlan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceRate",
                schema: "cpms",
                table: "PricingPlan",
                type: "decimal(10,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ProductTypeId",
                schema: "cpms",
                table: "PricingPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedOn",
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

            migrationBuilder.CreateIndex(
                name: "IX_PricingPlan_ProductTypeId",
                schema: "cpms",
                table: "PricingPlan",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CPConnector_PricingPlanId",
                schema: "cpms",
                table: "CPConnector",
                column: "PricingPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_CPConnector_PricingPlan_PricingPlanId",
                schema: "cpms",
                table: "CPConnector",
                column: "PricingPlanId",
                principalSchema: "cpms",
                principalTable: "PricingPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PricingPlan_ProductType_ProductTypeId",
                schema: "cpms",
                table: "PricingPlan",
                column: "ProductTypeId",
                principalSchema: "cpms",
                principalTable: "ProductType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PricingPlan_Unit_UnitId",
                schema: "cpms",
                table: "PricingPlan",
                column: "UnitId",
                principalSchema: "cpms",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

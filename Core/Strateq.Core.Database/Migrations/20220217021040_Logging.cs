using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strateq.Core.Database.Migrations
{
    public partial class Logging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cpms");

            migrationBuilder.CreateTable(
                name: "AuditLog",
                schema: "cpms",
                columns: table => new {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AffectedTable = table.Column<string>(type: "varchar(150)", nullable: true),
                    AffectedId = table.Column<long>(type: "bigint", nullable: true),
                    Operation = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogDetail",
                schema: "cpms",
                columns: table => new {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditLogId = table.Column<long>(type: "bigint", nullable: false),
                    ColumnName = table.Column<string>(type: "varchar(100)", nullable: true),
                    OriginalValue = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    NewValue = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AuditLogDetail", x => x.Id);
                });


            migrationBuilder.CreateTable(
                name: "RequestLog",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Controller = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Request = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemLog",
                schema: "cpms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestLogId = table.Column<long>(type: "bigint", nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Detail = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog",
                schema: "cpms");

            migrationBuilder.DropTable(
                name: "AuditLogDetail",
                schema: "cpms");

            migrationBuilder.DropTable(
                name: "RequestLog",
                schema: "cpms");

            migrationBuilder.DropTable(
                name: "SystemLog",
                schema: "cpms");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class LastWebMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeneFactorWelcomeMessage",
                schema: "web");

            migrationBuilder.AddColumn<string>(
                name: "WelcomeMessage",
                schema: "web",
                table: "BeneFactors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BeneFactorId",
                schema: "admin",
                table: "AccountsImportMony",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BeneFactorTypeId",
                schema: "admin",
                table: "AccountsImportMony",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BeneFactorId",
                schema: "admin",
                table: "AccountsExportMony",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BeneFactorTypeId",
                schema: "admin",
                table: "AccountsExportMony",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WelcomeMessage",
                schema: "web",
                table: "BeneFactors");

            migrationBuilder.DropColumn(
                name: "BeneFactorId",
                schema: "admin",
                table: "AccountsImportMony");

            migrationBuilder.DropColumn(
                name: "BeneFactorTypeId",
                schema: "admin",
                table: "AccountsImportMony");

            migrationBuilder.DropColumn(
                name: "BeneFactorId",
                schema: "admin",
                table: "AccountsExportMony");

            migrationBuilder.DropColumn(
                name: "BeneFactorTypeId",
                schema: "admin",
                table: "AccountsExportMony");

            migrationBuilder.CreateTable(
                name: "BeneFactorWelcomeMessage",
                schema: "web",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneFactorWelcomeMessage", x => x.Id);
                });
        }
    }
}

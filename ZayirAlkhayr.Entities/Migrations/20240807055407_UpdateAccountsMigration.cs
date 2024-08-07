using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class UpdateAccountsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "admin",
                table: "AccountsImportMony",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateUser",
                schema: "admin",
                table: "AccountsImportMony",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "admin",
                table: "AccountsExportMony",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateUser",
                schema: "admin",
                table: "AccountsExportMony",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "admin",
                table: "AccountsImportMony");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                schema: "admin",
                table: "AccountsImportMony");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "admin",
                table: "AccountsExportMony");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                schema: "admin",
                table: "AccountsExportMony");
        }
    }
}

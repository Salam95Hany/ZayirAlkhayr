using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class UpdateFamilyCategoryMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                schema: "admin",
                table: "FamilyCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertUser",
                schema: "admin",
                table: "FamilyCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "admin",
                table: "FamilyCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateUser",
                schema: "admin",
                table: "FamilyCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertDate",
                schema: "admin",
                table: "FamilyCategories");

            migrationBuilder.DropColumn(
                name: "InsertUser",
                schema: "admin",
                table: "FamilyCategories");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "admin",
                table: "FamilyCategories");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                schema: "admin",
                table: "FamilyCategories");
        }
    }
}

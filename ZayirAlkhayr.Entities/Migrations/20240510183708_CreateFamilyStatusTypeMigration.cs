using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class CreateFamilyStatusTypeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                schema: "web",
                table: "Photos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InsertUser",
                schema: "web",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                schema: "web",
                table: "Photos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "web",
                table: "Photos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdateUser",
                schema: "web",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FamilyCategoryId",
                schema: "admin",
                table: "FamilyStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FamilyStatusTypeId",
                schema: "admin",
                table: "FamilyStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FamilyCategories",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyStatusTypes",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyStatusTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyCategories",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "FamilyStatusTypes",
                schema: "admin");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                schema: "web",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "InsertUser",
                schema: "web",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                schema: "web",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "web",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                schema: "web",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "FamilyCategoryId",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "FamilyStatusTypeId",
                schema: "admin",
                table: "FamilyStatus");
        }
    }
}

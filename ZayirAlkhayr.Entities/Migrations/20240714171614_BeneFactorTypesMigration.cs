using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class BeneFactorTypesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                schema: "web",
                table: "BeneFactorTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertUser",
                schema: "web",
                table: "BeneFactorTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "web",
                table: "BeneFactorTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateUser",
                schema: "web",
                table: "BeneFactorTypes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertDate",
                schema: "web",
                table: "BeneFactorTypes");

            migrationBuilder.DropColumn(
                name: "InsertUser",
                schema: "web",
                table: "BeneFactorTypes");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "web",
                table: "BeneFactorTypes");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                schema: "web",
                table: "BeneFactorTypes");
        }
    }
}

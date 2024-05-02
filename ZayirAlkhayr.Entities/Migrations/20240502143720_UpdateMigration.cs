using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class UpdateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                schema: "web",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "InsertUser",
                schema: "web",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                schema: "web",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "web",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdateUser",
                schema: "web",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                schema: "web",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "InsertUser",
                schema: "web",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                schema: "web",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "web",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdateUser",
                schema: "web",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertDate",
                schema: "web",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "InsertUser",
                schema: "web",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                schema: "web",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "web",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                schema: "web",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                schema: "web",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "InsertUser",
                schema: "web",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                schema: "web",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "web",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                schema: "web",
                table: "Activities");
        }
    }
}

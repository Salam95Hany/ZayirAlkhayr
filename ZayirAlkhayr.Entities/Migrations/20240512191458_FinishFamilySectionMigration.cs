using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class FinishFamilySectionMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                schema: "admin",
                table: "FamilyStatus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InsertUser",
                schema: "admin",
                table: "FamilyStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                schema: "admin",
                table: "FamilyStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonOfRefuse",
                schema: "admin",
                table: "FamilyStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "admin",
                table: "FamilyStatus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdateUser",
                schema: "admin",
                table: "FamilyStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastVisitDate",
                schema: "admin",
                table: "FamilyExtraDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalPapers",
                schema: "admin",
                table: "FamilyExtraDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertDate",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "InsertUser",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "Nationality",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "ReasonOfRefuse",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "LastVisitDate",
                schema: "admin",
                table: "FamilyExtraDetails");

            migrationBuilder.DropColumn(
                name: "PersonalPapers",
                schema: "admin",
                table: "FamilyExtraDetails");
        }
    }
}

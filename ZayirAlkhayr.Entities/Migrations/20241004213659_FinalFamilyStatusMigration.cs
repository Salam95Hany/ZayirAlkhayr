using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class FinalFamilyStatusMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientType",
                schema: "admin",
                table: "FamilyPatient");

            migrationBuilder.AddColumn<bool>(
                name: "IsNeedProcess",
                schema: "admin",
                table: "FamilyPatient",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientTypeId",
                schema: "admin",
                table: "FamilyPatient",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                schema: "admin",
                table: "FamilyNeeds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWaiting",
                schema: "admin",
                table: "FamilyNeeds",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNeedProcess",
                schema: "admin",
                table: "FamilyPatient");

            migrationBuilder.DropColumn(
                name: "PatientTypeId",
                schema: "admin",
                table: "FamilyPatient");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                schema: "admin",
                table: "FamilyNeeds");

            migrationBuilder.DropColumn(
                name: "IsWaiting",
                schema: "admin",
                table: "FamilyNeeds");

            migrationBuilder.AddColumn<string>(
                name: "PatientType",
                schema: "admin",
                table: "FamilyPatient",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class UpdateFamilyStatusMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nationality",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.RenameColumn(
                name: "FamilyStatusTypeId",
                schema: "admin",
                table: "FamilyStatus",
                newName: "StatusTypeId");

            migrationBuilder.RenameColumn(
                name: "FamilyCategoryId",
                schema: "admin",
                table: "FamilyStatus",
                newName: "NationalityId");

            migrationBuilder.RenameColumn(
                name: "NeedTypeName",
                schema: "admin",
                table: "FamilyNeedTypes",
                newName: "UpdateUser");

            migrationBuilder.RenameColumn(
                name: "Category",
                schema: "admin",
                table: "FamilyNeedTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FamilyStatusId",
                schema: "admin",
                table: "FamilyNeeds",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "FamilyNeedTypeId",
                schema: "admin",
                table: "FamilyNeeds",
                newName: "NeedTypeId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "admin",
                table: "FamilyStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                schema: "admin",
                table: "FamilyNeedTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertUser",
                schema: "admin",
                table: "FamilyNeedTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "admin",
                table: "FamilyNeedTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Installment_debts",
                schema: "admin",
                table: "FamilyExpenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FamilyNationalities",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyNationalities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyNationalities",
                schema: "admin");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                schema: "admin",
                table: "FamilyNeedTypes");

            migrationBuilder.DropColumn(
                name: "InsertUser",
                schema: "admin",
                table: "FamilyNeedTypes");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "admin",
                table: "FamilyNeedTypes");

            migrationBuilder.DropColumn(
                name: "Installment_debts",
                schema: "admin",
                table: "FamilyExpenses");

            migrationBuilder.RenameColumn(
                name: "StatusTypeId",
                schema: "admin",
                table: "FamilyStatus",
                newName: "FamilyStatusTypeId");

            migrationBuilder.RenameColumn(
                name: "NationalityId",
                schema: "admin",
                table: "FamilyStatus",
                newName: "FamilyCategoryId");

            migrationBuilder.RenameColumn(
                name: "UpdateUser",
                schema: "admin",
                table: "FamilyNeedTypes",
                newName: "NeedTypeName");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "admin",
                table: "FamilyNeedTypes",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                schema: "admin",
                table: "FamilyNeeds",
                newName: "FamilyStatusId");

            migrationBuilder.RenameColumn(
                name: "NeedTypeId",
                schema: "admin",
                table: "FamilyNeeds",
                newName: "FamilyNeedTypeId");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                schema: "admin",
                table: "FamilyStatus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

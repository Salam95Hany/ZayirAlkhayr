using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class EditFamilyStatusTableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "StatusTypeId",
                schema: "admin",
                table: "FamilyStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "admin",
                table: "FamilyStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusTypeId",
                schema: "admin",
                table: "FamilyStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

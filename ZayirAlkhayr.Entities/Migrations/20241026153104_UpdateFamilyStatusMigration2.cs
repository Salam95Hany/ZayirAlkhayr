using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class UpdateFamilyStatusMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                schema: "admin",
                table: "FamilyStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                schema: "admin",
                table: "FamilyStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Jop",
                schema: "admin",
                table: "FamilyStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                schema: "admin",
                table: "FamilyStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Relevance",
                schema: "admin",
                table: "FamilyStatus",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "Education",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "Jop",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                schema: "admin",
                table: "FamilyStatus");

            migrationBuilder.DropColumn(
                name: "Relevance",
                schema: "admin",
                table: "FamilyStatus");
        }
    }
}

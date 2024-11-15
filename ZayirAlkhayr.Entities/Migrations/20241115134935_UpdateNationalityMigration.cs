using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class UpdateNationalityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nationality",
                schema: "admin",
                table: "Orphans");

            migrationBuilder.AddColumn<bool>(
                name: "IsGuaranteed",
                schema: "admin",
                table: "Orphans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NationalityId",
                schema: "admin",
                table: "Orphans",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGuaranteed",
                schema: "admin",
                table: "Orphans");

            migrationBuilder.DropColumn(
                name: "NationalityId",
                schema: "admin",
                table: "Orphans");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                schema: "admin",
                table: "Orphans",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

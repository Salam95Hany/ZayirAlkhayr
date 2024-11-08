using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class SortingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                schema: "web",
                table: "PhotoDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                schema: "web",
                table: "EventSliderImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                schema: "web",
                table: "ActivitiesSliderImage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                schema: "web",
                table: "PhotoDetails");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                schema: "web",
                table: "EventSliderImages");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                schema: "web",
                table: "ActivitiesSliderImage");
        }
    }
}

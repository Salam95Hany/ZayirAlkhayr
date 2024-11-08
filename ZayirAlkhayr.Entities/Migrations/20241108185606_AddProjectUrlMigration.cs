using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class AddProjectUrlMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectUrl",
                schema: "web",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectUrl",
                schema: "web",
                table: "Projects");
        }
    }
}

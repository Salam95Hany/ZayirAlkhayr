using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class AddPhotoTableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "FamilyNeedTypes",
                newName: "FamilyNeedTypes",
                newSchema: "admin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "FamilyNeedTypes",
                schema: "admin",
                newName: "FamilyNeedTypes");
        }
    }
}

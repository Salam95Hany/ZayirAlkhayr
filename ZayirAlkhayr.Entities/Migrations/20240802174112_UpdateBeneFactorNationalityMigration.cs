using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class UpdateBeneFactorNationalityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nationality",
                schema: "web",
                table: "BeneFactors");

            migrationBuilder.AddColumn<int>(
                name: "NationalityId",
                schema: "web",
                table: "BeneFactors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalityId",
                schema: "web",
                table: "BeneFactors");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                schema: "web",
                table: "BeneFactors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

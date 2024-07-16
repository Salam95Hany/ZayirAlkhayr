using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class FinalBeneFactorMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeneFactorTypeId",
                schema: "web",
                table: "BeneFactorValues");

            migrationBuilder.RenameColumn(
                name: "DetailType",
                schema: "web",
                table: "BeneFactorDetails",
                newName: "Details");

            migrationBuilder.AlterColumn<int>(
                name: "BeneFactorValueId",
                schema: "web",
                table: "BeneFactorDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BeneFactorId",
                schema: "web",
                table: "BeneFactorDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BeneFactorTypeId",
                schema: "web",
                table: "BeneFactorDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeneFactorId",
                schema: "web",
                table: "BeneFactorDetails");

            migrationBuilder.DropColumn(
                name: "BeneFactorTypeId",
                schema: "web",
                table: "BeneFactorDetails");

            migrationBuilder.RenameColumn(
                name: "Details",
                schema: "web",
                table: "BeneFactorDetails",
                newName: "DetailType");

            migrationBuilder.AddColumn<int>(
                name: "BeneFactorTypeId",
                schema: "web",
                table: "BeneFactorValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "BeneFactorValueId",
                schema: "web",
                table: "BeneFactorDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

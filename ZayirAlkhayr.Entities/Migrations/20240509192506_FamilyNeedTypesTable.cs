using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class FamilyNeedTypesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElectricalAppliances",
                schema: "admin",
                table: "FamilyNeeds");

            migrationBuilder.DropColumn(
                name: "Furniture",
                schema: "admin",
                table: "FamilyNeeds");

            migrationBuilder.DropColumn(
                name: "HomeMaintenance",
                schema: "admin",
                table: "FamilyNeeds");

            migrationBuilder.DropColumn(
                name: "Joinary",
                schema: "admin",
                table: "FamilyNeeds");

            migrationBuilder.AddColumn<int>(
                name: "FamilyNeedTypeId",
                schema: "admin",
                table: "FamilyNeeds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FamilyNeedTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NeedTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyNeedTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyNeedTypes");

            migrationBuilder.DropColumn(
                name: "FamilyNeedTypeId",
                schema: "admin",
                table: "FamilyNeeds");

            migrationBuilder.AddColumn<string>(
                name: "ElectricalAppliances",
                schema: "admin",
                table: "FamilyNeeds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Furniture",
                schema: "admin",
                table: "FamilyNeeds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeMaintenance",
                schema: "admin",
                table: "FamilyNeeds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Joinary",
                schema: "admin",
                table: "FamilyNeeds",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

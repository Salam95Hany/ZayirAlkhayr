using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class AdjustmentTypeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdjustmentType",
                schema: "Inv",
                table: "InventoryAdjustments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjustmentType",
                schema: "Inv",
                table: "InventoryAdjustments");
        }
    }
}

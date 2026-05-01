using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class ActionIdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionId",
                schema: "Inv",
                table: "InventoryAdjustments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionId",
                schema: "Inv",
                table: "InventoryAdjustments");
        }
    }
}

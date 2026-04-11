using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class DeleteCategoryMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                schema: "POS",
                table: "Items");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                schema: "POS",
                table: "Items",
                column: "CategoryId",
                principalSchema: "POS",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                schema: "POS",
                table: "Items");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                schema: "POS",
                table: "Items",
                column: "CategoryId",
                principalSchema: "POS",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

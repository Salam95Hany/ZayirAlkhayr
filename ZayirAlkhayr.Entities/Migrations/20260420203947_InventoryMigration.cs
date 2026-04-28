using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class InventoryMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemRecipes",
                schema: "Inv",
                columns: table => new
                {
                    ItemRecipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    QuantityNeeded = table.Column<double>(type: "float", nullable: false),
                    InsertUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemRecipes", x => x.ItemRecipeId);
                    table.ForeignKey(
                        name: "FK_ItemRecipes_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "Inv",
                        principalTable: "InventoryItems",
                        principalColumn: "InventoryItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemRecipes_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "POS",
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemRecipes_InventoryItemId",
                schema: "Inv",
                table: "ItemRecipes",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemRecipes_ItemId_InventoryItemId",
                schema: "Inv",
                table: "ItemRecipes",
                columns: new[] { "ItemId", "InventoryItemId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemRecipes",
                schema: "Inv");
        }
    }
}

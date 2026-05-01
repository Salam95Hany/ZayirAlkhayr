using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class InventoryAdjustmentDetailsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                schema: "Inv",
                table: "InventoryAdjustments");

            migrationBuilder.RenameColumn(
                name: "QuantityChange",
                schema: "Inv",
                table: "InventoryAdjustments",
                newName: "TotalAffectedItems");

            migrationBuilder.AddColumn<string>(
                name: "PurchaseNumber",
                schema: "Inv",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                schema: "Inv",
                table: "InventoryAdjustments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "InventoryAdjustmentDetails",
                schema: "Inv",
                columns: table => new
                {
                    InventoryAdjustmentDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryAdjustmentId = table.Column<int>(type: "int", nullable: false),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    QuantityBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityChange = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAdjustmentDetails", x => x.InventoryAdjustmentDetailId);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustmentDetails_InventoryAdjustments_InventoryAdjustmentId",
                        column: x => x.InventoryAdjustmentId,
                        principalSchema: "Inv",
                        principalTable: "InventoryAdjustments",
                        principalColumn: "InventoryAdjustmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustmentDetails_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "Inv",
                        principalTable: "InventoryItems",
                        principalColumn: "InventoryItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustmentDetails_InventoryAdjustmentId",
                schema: "Inv",
                table: "InventoryAdjustmentDetails",
                column: "InventoryAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustmentDetails_InventoryItemId",
                schema: "Inv",
                table: "InventoryAdjustmentDetails",
                column: "InventoryItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryAdjustmentDetails",
                schema: "Inv");

            migrationBuilder.DropColumn(
                name: "PurchaseNumber",
                schema: "Inv",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "TotalAffectedItems",
                schema: "Inv",
                table: "InventoryAdjustments",
                newName: "QuantityChange");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                schema: "Inv",
                table: "InventoryAdjustments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "InventoryItemId",
                schema: "Inv",
                table: "InventoryAdjustments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

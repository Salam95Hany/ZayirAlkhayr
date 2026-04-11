using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderNumber",
                schema: "POS",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                schema: "POS",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoidNotes",
                schema: "POS",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoidReason",
                schema: "POS",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "POS",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "POS",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                schema: "POS",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ItemId",
                schema: "POS",
                table: "OrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                schema: "POS",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                schema: "POS",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                schema: "POS",
                table: "Items",
                column: "CategoryId",
                principalSchema: "POS",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Items_ItemId",
                schema: "POS",
                table: "OrderDetails",
                column: "ItemId",
                principalSchema: "POS",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                schema: "POS",
                table: "OrderDetails",
                column: "OrderId",
                principalSchema: "POS",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                schema: "POS",
                table: "Orders",
                column: "CustomerId",
                principalSchema: "Cust",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                schema: "POS",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Items_ItemId",
                schema: "POS",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                schema: "POS",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                schema: "POS",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                schema: "POS",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ItemId",
                schema: "POS",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                schema: "POS",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId",
                schema: "POS",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Note",
                schema: "POS",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoidNotes",
                schema: "POS",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoidReason",
                schema: "POS",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "POS",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "POS",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                schema: "POS",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

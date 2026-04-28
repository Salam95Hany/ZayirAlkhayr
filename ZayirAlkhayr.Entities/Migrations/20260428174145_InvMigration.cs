using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class InvMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Units",
                schema: "Inv",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_UnitId",
                schema: "Inv",
                table: "InventoryItems",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Units_UnitId",
                schema: "Inv",
                table: "InventoryItems",
                column: "UnitId",
                principalSchema: "Inv",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Units_UnitId",
                schema: "Inv",
                table: "InventoryItems");

            migrationBuilder.DropTable(
                name: "Units",
                schema: "Inv");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_UnitId",
                schema: "Inv",
                table: "InventoryItems");
        }
    }
}

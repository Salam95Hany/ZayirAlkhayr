using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class AddUserNameArMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InsertUser",
                schema: "POS",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNameAr",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_InsertUser",
                schema: "POS",
                table: "Orders",
                column: "InsertUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_InsertUser",
                schema: "POS",
                table: "Orders",
                column: "InsertUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_InsertUser",
                schema: "POS",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_InsertUser",
                schema: "POS",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserNameAr",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "InsertUser",
                schema: "POS",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}

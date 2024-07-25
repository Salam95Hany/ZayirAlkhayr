using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class UpdateBeneFactorStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeneFactorValues",
                schema: "web");

            migrationBuilder.RenameColumn(
                name: "BeneFactorValueId",
                schema: "web",
                table: "BeneFactorDetails",
                newName: "ParentId");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "web",
                table: "BeneFactorDetails",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "web",
                table: "BeneFactorDetails");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                schema: "web",
                table: "BeneFactorDetails",
                newName: "BeneFactorValueId");

            migrationBuilder.CreateTable(
                name: "BeneFactorValues",
                schema: "web",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeneFactorId = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalValue = table.Column<double>(type: "float", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneFactorValues", x => x.Id);
                });
        }
    }
}

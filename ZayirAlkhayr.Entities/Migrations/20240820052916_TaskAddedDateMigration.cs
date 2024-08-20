using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class TaskAddedDateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TaskAddedDate",
                schema: "admin",
                table: "GeneralTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskAddedDate",
                schema: "admin",
                table: "GeneralTasks");
        }
    }
}

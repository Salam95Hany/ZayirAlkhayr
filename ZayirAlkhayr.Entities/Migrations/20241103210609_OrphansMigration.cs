using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class OrphansMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orphans",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyStatusId = table.Column<int>(type: "int", nullable: false),
                    BenefactorId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Jop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Income = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicStage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyMembersCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankingBrothers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenefactorPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenefactorAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenefactorType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orphans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDetails",
                schema: "web",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "web",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDonationAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenefactorCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemainingAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    InsertUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orphans",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "ProjectDetails",
                schema: "web");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "web");
        }
    }
}

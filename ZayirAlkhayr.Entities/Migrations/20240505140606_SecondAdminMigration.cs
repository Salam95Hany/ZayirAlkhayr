using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZayirAlkhayr.Entities.Migrations
{
    public partial class SecondAdminMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.CreateTable(
                name: "FamilyDetails",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyStatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Relevance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Jop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildernsCount = table.Column<int>(type: "int", nullable: true),
                    FamilyMembersCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyExpenses",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyStatusId = table.Column<int>(type: "int", nullable: false),
                    Rent_Electricity_Water_Gas_Sewage = table.Column<int>(type: "int", nullable: true),
                    MedicalExamination_Treatment = table.Column<int>(type: "int", nullable: true),
                    SchoolExpenses = table.Column<int>(type: "int", nullable: false),
                    PhysiotherapySessions = table.Column<int>(type: "int", nullable: true),
                    Analysis = table.Column<int>(type: "int", nullable: true),
                    SatisfactoryTransfers = table.Column<int>(type: "int", nullable: true),
                    MedicalXRays = table.Column<int>(type: "int", nullable: true),
                    IsMinisterialSupply = table.Column<bool>(type: "bit", nullable: true),
                    IsFoodBank = table.Column<bool>(type: "bit", nullable: true),
                    TotalFamilyExpenses = table.Column<int>(type: "int", nullable: true),
                    NetFamilyIncome = table.Column<int>(type: "int", nullable: true),
                    FamilyCount = table.Column<int>(type: "int", nullable: true),
                    AvgPersonIncome = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyExpenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyExtraDetails",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyStatusId = table.Column<int>(type: "int", nullable: false),
                    StatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HousingNeedsAndStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearcherNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferencesNotes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyExtraDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyIncome",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyStatusId = table.Column<int>(type: "int", nullable: false),
                    FatherJop = table.Column<int>(type: "int", nullable: true),
                    MotherJop = table.Column<int>(type: "int", nullable: true),
                    ChildernsJop = table.Column<int>(type: "int", nullable: true),
                    AffairSpension_SocialSolidarity = table.Column<int>(type: "int", nullable: true),
                    Project = table.Column<int>(type: "int", nullable: true),
                    LiveStock_Lands = table.Column<int>(type: "int", nullable: true),
                    Organization_ZakatCommittee = table.Column<int>(type: "int", nullable: true),
                    InsurancePension = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalFamilyIncome = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyIncome", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyNeeds",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyStatusId = table.Column<int>(type: "int", nullable: false),
                    ElectricalAppliances = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Furniture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeMaintenance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Joinary = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyNeeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyPatient",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyStatusId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMedicalReport = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyPatient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyStatus",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Center = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportingParty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusTypeId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyStatus", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyDetails",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "FamilyExpenses",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "FamilyExtraDetails",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "FamilyIncome",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "FamilyNeeds",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "FamilyPatient",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "FamilyStatus",
                schema: "admin");
        }
    }
}

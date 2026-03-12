using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class _120320261633 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PmTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsUsingStaff = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsBeforeStartStaff = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsManager = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsFaaliyet2Control = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsOnlyOneStatu = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    StatusQuota = table.Column<int>(type: "int", nullable: false),
                    IsDeteled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PersonnelMovements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Finish = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ContractStart = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ContractFinish = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PmTypeId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    BranchId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    HealthFacilityId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    AfiliatedUnitId = table.Column<int>(type: "int", nullable: false),
                    PersonnelId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    IsUsingQuota = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSgk = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeteled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonnelMovements_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonnelMovements_HealthFacilities_HealthFacilityId",
                        column: x => x.HealthFacilityId,
                        principalTable: "HealthFacilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonnelMovements_Personnel_PersonnelId",
                        column: x => x.PersonnelId,
                        principalTable: "Personnel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonnelMovements_PmTypes_PmTypeId",
                        column: x => x.PmTypeId,
                        principalTable: "PmTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelMovements_BranchId",
                table: "PersonnelMovements",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelMovements_HealthFacilityId",
                table: "PersonnelMovements",
                column: "HealthFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelMovements_PersonnelId",
                table: "PersonnelMovements",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelMovements_PmTypeId",
                table: "PersonnelMovements",
                column: "PmTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonnelMovements");

            migrationBuilder.DropTable(
                name: "PmTypes");
        }
    }
}

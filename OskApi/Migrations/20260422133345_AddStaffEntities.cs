using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HealthFacilityTypes",
                keyColumn: "Code",
                keyValue: null,
                column: "Code",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "HealthFacilityTypes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    BranchId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    HealthFacilityId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeteled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Staff_HealthFacilities_HealthFacilityId",
                        column: x => x.HealthFacilityId,
                        principalTable: "HealthFacilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TemporarayStaff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    BranchId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    HealthFacilityId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    PmTypeId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeteled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporarayStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporarayStaff_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemporarayStaff_HealthFacilities_HealthFacilityId",
                        column: x => x.HealthFacilityId,
                        principalTable: "HealthFacilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemporarayStaff_PmTypes_PmTypeId",
                        column: x => x.PmTypeId,
                        principalTable: "PmTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_BranchId",
                table: "Staff",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_HealthFacilityId",
                table: "Staff",
                column: "HealthFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarayStaff_BranchId",
                table: "TemporarayStaff",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarayStaff_HealthFacilityId",
                table: "TemporarayStaff",
                column: "HealthFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarayStaff_PmTypeId",
                table: "TemporarayStaff",
                column: "PmTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "TemporarayStaff");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "HealthFacilityTypes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}

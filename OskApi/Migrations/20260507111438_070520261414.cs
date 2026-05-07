using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class _070520261414 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonnelMovementSubFacilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    PersonnelMovementId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    SubFacilityId = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeteled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelMovementSubFacilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonnelMovementSubFacilities_HealthFacilities_SubFacilityId",
                        column: x => x.SubFacilityId,
                        principalTable: "HealthFacilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonnelMovementSubFacilities_PersonnelMovements_PersonnelM~",
                        column: x => x.PersonnelMovementId,
                        principalTable: "PersonnelMovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelMovementSubFacilities_PersonnelMovementId",
                table: "PersonnelMovementSubFacilities",
                column: "PersonnelMovementId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelMovementSubFacilities_SubFacilityId",
                table: "PersonnelMovementSubFacilities",
                column: "SubFacilityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonnelMovementSubFacilities");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class _130320261024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AfiliatedUnitId",
                table: "PersonnelMovements",
                type: "char(36)",
                maxLength: 36,
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AfiliatedUnitId",
                table: "PersonnelMovements",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldMaxLength: 36)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }
    }
}

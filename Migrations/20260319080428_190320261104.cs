using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class _190320261104 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMainSpecialty",
                table: "Branches");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Titles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PmTypes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PersonnelMovements",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PersonnelBranches",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Personnel",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "IcBeds",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "IcBedNames",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Branches",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PmTypes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PersonnelMovements");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PersonnelBranches");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "IcBeds");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "IcBedNames");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Branches");

            migrationBuilder.AddColumn<bool>(
                name: "IsMainSpecialty",
                table: "Branches",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}

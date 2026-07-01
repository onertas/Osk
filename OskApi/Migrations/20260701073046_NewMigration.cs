using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "Titles",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "TemporarayStaff",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "Staff",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "PmTypes",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "PersonnelMovementSubFacilities",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "PersonnelMovements",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "PersonnelBranches",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "Personnel",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "IcBeds",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "IcBedNames",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "HealthFacilityTypes",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "HealthFacilities",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDeteled",
                table: "Branches",
                newName: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Titles",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "TemporarayStaff",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Staff",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "PmTypes",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "PersonnelMovementSubFacilities",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "PersonnelMovements",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "PersonnelBranches",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Personnel",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "IcBeds",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "IcBedNames",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "HealthFacilityTypes",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "HealthFacilities",
                newName: "IsDeteled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Branches",
                newName: "IsDeteled");
        }
    }
}

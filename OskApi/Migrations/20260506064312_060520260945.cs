using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class _060520260945 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowBed",
                table: "HealthFacilityTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowDevice",
                table: "HealthFacilityTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowStaff",
                table: "HealthFacilityTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowTempStaff",
                table: "HealthFacilityTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_IcBeds_HealthFacilityId",
                table: "IcBeds",
                column: "HealthFacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_IcBeds_HealthFacilities_HealthFacilityId",
                table: "IcBeds",
                column: "HealthFacilityId",
                principalTable: "HealthFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IcBeds_HealthFacilities_HealthFacilityId",
                table: "IcBeds");

            migrationBuilder.DropIndex(
                name: "IX_IcBeds_HealthFacilityId",
                table: "IcBeds");

            migrationBuilder.DropColumn(
                name: "ShowBed",
                table: "HealthFacilityTypes");

            migrationBuilder.DropColumn(
                name: "ShowDevice",
                table: "HealthFacilityTypes");

            migrationBuilder.DropColumn(
                name: "ShowStaff",
                table: "HealthFacilityTypes");

            migrationBuilder.DropColumn(
                name: "ShowTempStaff",
                table: "HealthFacilityTypes");
        }
    }
}

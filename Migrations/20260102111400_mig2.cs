using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorporationName",
                table: "HealthFacilities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxNumber",
                table: "HealthFacilities",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorporationName",
                table: "HealthFacilities");

            migrationBuilder.DropColumn(
                name: "TaxNumber",
                table: "HealthFacilities");
        }
    }
}

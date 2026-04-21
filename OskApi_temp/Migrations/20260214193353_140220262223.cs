using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class _140220262223 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "HealthFacilityTypes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "HealthFacilityTypes");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class AddOpeningDateToHealthFacility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OpeningDate",
                table: "HealthFacilities",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpeningDate",
                table: "HealthFacilities");
        }
    }
}

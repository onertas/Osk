using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OskApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsOnlyOneStatu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnlyOneStatu",
                table: "PmTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnlyOneStatu",
                table: "PmTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}

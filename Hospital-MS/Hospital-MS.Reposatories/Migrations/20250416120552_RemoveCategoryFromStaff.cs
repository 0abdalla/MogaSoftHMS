using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Reposatories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoryFromStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Staff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Staff",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

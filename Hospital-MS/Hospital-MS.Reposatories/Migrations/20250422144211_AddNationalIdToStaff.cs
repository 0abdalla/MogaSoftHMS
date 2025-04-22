using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Reposatories.Migrations
{
    /// <inheritdoc />
    public partial class AddNationalIdToStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "Staff",
                type: "nvarchar(22)",
                maxLength: 22,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Staff");
        }
    }
}

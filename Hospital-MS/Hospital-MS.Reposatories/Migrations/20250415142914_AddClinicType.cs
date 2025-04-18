using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Reposatories.Migrations
{
    /// <inheritdoc />
    public partial class AddClinicType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Clinics",
                type: "nvarchar(55)",
                maxLength: 55,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Clinics");
        }
    }
}

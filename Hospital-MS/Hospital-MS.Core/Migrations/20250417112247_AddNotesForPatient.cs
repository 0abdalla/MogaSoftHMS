using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Reposatories.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesForPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Patients",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Patients");
        }
    }
}

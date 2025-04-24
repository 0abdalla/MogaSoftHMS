using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Reposatories.Migrations
{
    /// <inheritdoc />
    public partial class AddEmergencyPhone02ToPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmergencyPhone",
                table: "Patients",
                newName: "EmergencyPhone02");

            migrationBuilder.RenameColumn(
                name: "EmergencyContact",
                table: "Patients",
                newName: "EmergencyContact02");

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact01",
                table: "Patients",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyPhone01",
                table: "Patients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmergencyContact01",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "EmergencyPhone01",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "EmergencyPhone02",
                table: "Patients",
                newName: "EmergencyPhone");

            migrationBuilder.RenameColumn(
                name: "EmergencyContact02",
                table: "Patients",
                newName: "EmergencyContact");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Reposatories.Migrations
{
    /// <inheritdoc />
    public partial class EditAdmissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiagnosisOnAdmission",
                table: "Admissions");

            migrationBuilder.AddColumn<string>(
                name: "CompanionName",
                table: "Admissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanionNationalId",
                table: "Admissions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanionPhone",
                table: "Admissions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasCompanion",
                table: "Admissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HealthStatus",
                table: "Admissions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InitialDiagnosis",
                table: "Admissions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanionName",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "CompanionNationalId",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "CompanionPhone",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "HasCompanion",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "HealthStatus",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "InitialDiagnosis",
                table: "Admissions");

            migrationBuilder.AddColumn<string>(
                name: "DiagnosisOnAdmission",
                table: "Admissions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}

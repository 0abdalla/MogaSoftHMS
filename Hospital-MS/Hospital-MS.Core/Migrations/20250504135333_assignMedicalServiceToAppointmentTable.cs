using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class assignMedicalServiceToAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicalServiceId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MedicalServiceId",
                table: "Appointments",
                column: "MedicalServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_MedicalServices_MedicalServiceId",
                table: "Appointments",
                column: "MedicalServiceId",
                principalTable: "MedicalServices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_MedicalServices_MedicalServiceId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_MedicalServiceId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MedicalServiceId",
                table: "Appointments");
        }
    }
}

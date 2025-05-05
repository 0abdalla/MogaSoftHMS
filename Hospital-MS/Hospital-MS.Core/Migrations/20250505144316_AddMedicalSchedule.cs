using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalServiceSchedule_MedicalServices_MedicalServiceId",
                table: "MedicalServiceSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalServiceSchedule",
                table: "MedicalServiceSchedule");

            migrationBuilder.RenameTable(
                name: "MedicalServiceSchedule",
                newName: "MedicalServiceSchedules");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalServiceSchedule_MedicalServiceId",
                table: "MedicalServiceSchedules",
                newName: "IX_MedicalServiceSchedules_MedicalServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalServiceSchedules",
                table: "MedicalServiceSchedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalServiceSchedules_MedicalServices_MedicalServiceId",
                table: "MedicalServiceSchedules",
                column: "MedicalServiceId",
                principalTable: "MedicalServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalServiceSchedules_MedicalServices_MedicalServiceId",
                table: "MedicalServiceSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalServiceSchedules",
                table: "MedicalServiceSchedules");

            migrationBuilder.RenameTable(
                name: "MedicalServiceSchedules",
                newName: "MedicalServiceSchedule");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalServiceSchedules_MedicalServiceId",
                table: "MedicalServiceSchedule",
                newName: "IX_MedicalServiceSchedule_MedicalServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalServiceSchedule",
                table: "MedicalServiceSchedule",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalServiceSchedule_MedicalServices_MedicalServiceId",
                table: "MedicalServiceSchedule",
                column: "MedicalServiceId",
                principalTable: "MedicalServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

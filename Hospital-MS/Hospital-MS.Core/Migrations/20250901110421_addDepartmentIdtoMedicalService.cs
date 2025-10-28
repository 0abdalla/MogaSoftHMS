using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class addDepartmentIdtoMedicalService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "surgeryType",
                table: "Admissions");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "MedicalServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MedicalServiceId",
                table: "Admissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServices_DepartmentId",
                table: "MedicalServices",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_MedicalServiceId",
                table: "Admissions",
                column: "MedicalServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_MedicalServices_MedicalServiceId",
                table: "Admissions",
                column: "MedicalServiceId",
                principalTable: "MedicalServices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalServices_Departments_DepartmentId",
                table: "MedicalServices",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_MedicalServices_MedicalServiceId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalServices_Departments_DepartmentId",
                table: "MedicalServices");

            migrationBuilder.DropIndex(
                name: "IX_MedicalServices_DepartmentId",
                table: "MedicalServices");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_MedicalServiceId",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "MedicalServices");

            migrationBuilder.DropColumn(
                name: "MedicalServiceId",
                table: "Admissions");

            migrationBuilder.AddColumn<string>(
                name: "surgeryType",
                table: "Admissions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class handleRelationBetweenAdmissionAndDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Staff_DoctorId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTitles_Departments_DepartmentId",
                table: "JobTitles");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "JobTitles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Doctors_DoctorId",
                table: "Admissions",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobTitles_Departments_DepartmentId",
                table: "JobTitles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Doctors_DoctorId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTitles_Departments_DepartmentId",
                table: "JobTitles");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "JobTitles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Staff_DoctorId",
                table: "Admissions",
                column: "DoctorId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobTitles_Departments_DepartmentId",
                table: "JobTitles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

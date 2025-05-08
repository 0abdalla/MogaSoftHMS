using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AssignJobDepartmentToStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobDepartmentId",
                table: "Staff",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobDepartmentId",
                table: "Staff",
                column: "JobDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_JobDepartment_JobDepartmentId",
                table: "Staff",
                column: "JobDepartmentId",
                principalTable: "JobDepartment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_JobDepartment_JobDepartmentId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_JobDepartmentId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "JobDepartmentId",
                table: "Staff");
        }
    }
}

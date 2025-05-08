using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClincFromStaffTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Departments_DepartmentId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_DepartmentId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Staff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Staff",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Staff",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_DepartmentId",
                table: "Staff",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Departments_DepartmentId",
                table: "Staff",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}

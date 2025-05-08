using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddJobDepartmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobTitles_Departments_DepartmentId",
                table: "JobTitles");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "JobTitles",
                newName: "JobDepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_JobTitles_DepartmentId",
                table: "JobTitles",
                newName: "IX_JobTitles_JobDepartmentId");

            migrationBuilder.CreateTable(
                name: "JobDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDepartment_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobDepartment_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobDepartment_CreatedById",
                table: "JobDepartment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobDepartment_UpdatedById",
                table: "JobDepartment",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_JobTitles_JobDepartment_JobDepartmentId",
                table: "JobTitles",
                column: "JobDepartmentId",
                principalTable: "JobDepartment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobTitles_JobDepartment_JobDepartmentId",
                table: "JobTitles");

            migrationBuilder.DropTable(
                name: "JobDepartment");

            migrationBuilder.RenameColumn(
                name: "JobDepartmentId",
                table: "JobTitles",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_JobTitles_JobDepartmentId",
                table: "JobTitles",
                newName: "IX_JobTitles_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobTitles_Departments_DepartmentId",
                table: "JobTitles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Reposatories.Migrations
{
    /// <inheritdoc />
    public partial class AddDepatrmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_StaffCategories_CategoryId",
                table: "Staff");

            migrationBuilder.DropTable(
                name: "StaffCategories");

            migrationBuilder.DropIndex(
                name: "IX_Staff_CategoryId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DischargeDateTime",
                table: "Admissions");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Staff",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Admissions",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(750)",
                oldMaxLength: 750);

            migrationBuilder.AddColumn<DateTime>(
                name: "AdmissionDate",
                table: "Admissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Admissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Admissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Admissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_DepartmentId",
                table: "Staff",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_DepartmentId",
                table: "Admissions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_DoctorId",
                table: "Admissions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_RoomId",
                table: "Admissions",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedById",
                table: "Departments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UpdatedById",
                table: "Departments",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Departments_DepartmentId",
                table: "Admissions",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Rooms_RoomId",
                table: "Admissions",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Staff_DoctorId",
                table: "Admissions",
                column: "DoctorId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Departments_DepartmentId",
                table: "Staff",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Departments_DepartmentId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Rooms_RoomId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Staff_DoctorId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Departments_DepartmentId",
                table: "Staff");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Staff_DepartmentId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_DepartmentId",
                table: "Admissions");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_DoctorId",
                table: "Admissions");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_RoomId",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "AdmissionDate",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Admissions");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Admissions",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(750)",
                oldMaxLength: 750,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DischargeDateTime",
                table: "Admissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StaffCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffCategories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffCategories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_CategoryId",
                table: "Staff",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffCategories_CreatedById",
                table: "StaffCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StaffCategories_UpdatedById",
                table: "StaffCategories",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_StaffCategories_CategoryId",
                table: "Staff",
                column: "CategoryId",
                principalTable: "StaffCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

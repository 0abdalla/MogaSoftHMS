using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddHRTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Departments_DepartmentId",
                table: "Staff");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Staff",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "JobLevelId",
                table: "Staff",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobTitleId",
                table: "Staff",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobTypeId",
                table: "Staff",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobLevels",
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
                    table.PrimaryKey("PK_JobLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobLevels_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobLevels_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTitles_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobTitles_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobTitles_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobTypes",
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
                    table.PrimaryKey("PK_JobTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTypes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobTypes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobLevelId",
                table: "Staff",
                column: "JobLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobTitleId",
                table: "Staff",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_JobTypeId",
                table: "Staff",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLevels_CreatedById",
                table: "JobLevels",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobLevels_UpdatedById",
                table: "JobLevels",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_CreatedById",
                table: "JobTitles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_DepartmentId",
                table: "JobTitles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_UpdatedById",
                table: "JobTitles",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTypes_CreatedById",
                table: "JobTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobTypes_UpdatedById",
                table: "JobTypes",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Departments_DepartmentId",
                table: "Staff",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_JobLevels_JobLevelId",
                table: "Staff",
                column: "JobLevelId",
                principalTable: "JobLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_JobTitles_JobTitleId",
                table: "Staff",
                column: "JobTitleId",
                principalTable: "JobTitles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_JobTypes_JobTypeId",
                table: "Staff",
                column: "JobTypeId",
                principalTable: "JobTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Departments_DepartmentId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_JobLevels_JobLevelId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_JobTitles_JobTitleId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_JobTypes_JobTypeId",
                table: "Staff");

            migrationBuilder.DropTable(
                name: "JobLevels");

            migrationBuilder.DropTable(
                name: "JobTitles");

            migrationBuilder.DropTable(
                name: "JobTypes");

            migrationBuilder.DropIndex(
                name: "IX_Staff_JobLevelId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_JobTitleId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_JobTypeId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "JobLevelId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "JobTitleId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "JobTypeId",
                table: "Staff");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Staff",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Departments_DepartmentId",
                table: "Staff",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

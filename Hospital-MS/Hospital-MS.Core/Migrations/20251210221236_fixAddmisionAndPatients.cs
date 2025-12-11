using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class fixAddmisionAndPatients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Doctors_DoctorId",
                table: "Admissions");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Admissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AdmissionType",
                table: "Admissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Admissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdmissionCharge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdmissionId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChargeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionCharge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdmissionCharge_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionCharge_AdmissionId",
                table: "AdmissionCharge",
                column: "AdmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Doctors_DoctorId",
                table: "Admissions",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Doctors_DoctorId",
                table: "Admissions");

            migrationBuilder.DropTable(
                name: "AdmissionCharge");

            migrationBuilder.DropColumn(
                name: "AdmissionType",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Admissions");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Admissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Doctors_DoctorId",
                table: "Admissions",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

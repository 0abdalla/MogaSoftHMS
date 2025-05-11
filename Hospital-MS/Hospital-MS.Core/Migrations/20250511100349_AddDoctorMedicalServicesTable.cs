using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorMedicalServicesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_MedicalServices_MedicalServiceId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_MedicalServiceId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "MedicalServiceId",
                table: "Doctors");

            migrationBuilder.CreateTable(
                name: "DoctorMedicalService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    MedicalServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorMedicalService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorMedicalService_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoctorMedicalService_MedicalServices_MedicalServiceId",
                        column: x => x.MedicalServiceId,
                        principalTable: "MedicalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalService_DoctorId",
                table: "DoctorMedicalService",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalService_MedicalServiceId",
                table: "DoctorMedicalService",
                column: "MedicalServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorMedicalService");

            migrationBuilder.AddColumn<int>(
                name: "MedicalServiceId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_MedicalServiceId",
                table: "Doctors",
                column: "MedicalServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_MedicalServices_MedicalServiceId",
                table: "Doctors",
                column: "MedicalServiceId",
                principalTable: "MedicalServices",
                principalColumn: "Id");
        }
    }
}

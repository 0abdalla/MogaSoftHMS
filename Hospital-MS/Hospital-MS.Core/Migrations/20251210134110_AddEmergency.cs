using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_MS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddEmergency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmergencyVisit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ChiefComplaint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodPressure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeartRate = table.Column<int>(type: "int", nullable: true),
                    RespiratoryRate = table.Column<int>(type: "int", nullable: true),
                    Temperature = table.Column<float>(type: "real", nullable: true),
                    OxygenSaturation = table.Column<int>(type: "int", nullable: true),
                    PainScore = table.Column<int>(type: "int", nullable: true),
                    Allergies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssessmentNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TreatmentNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanionPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanionNationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorId = table.Column<int>(type: "int", nullable: true),
                    IsAdmitted = table.Column<bool>(type: "bit", nullable: false),
                    AdmissionId = table.Column<int>(type: "int", nullable: true),
                    DischargeTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DischargeNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastStatusUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EncounterNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyVisit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyVisit_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "Admissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmergencyVisit_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmergencyVisit_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmergencyVisit_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmergencyVisit_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyVisit_AdmissionId",
                table: "EmergencyVisit",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyVisit_CreatedById",
                table: "EmergencyVisit",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyVisit_DoctorId",
                table: "EmergencyVisit",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyVisit_PatientId",
                table: "EmergencyVisit",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyVisit_UpdatedById",
                table: "EmergencyVisit",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmergencyVisit");
        }
    }
}

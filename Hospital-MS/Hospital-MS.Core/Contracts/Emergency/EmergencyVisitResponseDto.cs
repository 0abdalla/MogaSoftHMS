using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Emergency;
public class EmergencyVisitResponseDto
{
    public int Id { get; set; }
    public Guid EncounterNumber { get; set; }

    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;

    public DateTime ArrivalTime { get; set; }
    public string? ChiefComplaint { get; set; }

    public EmergencySeverity Severity { get; set; }
    public EmergencyStatus Status { get; set; }
    public string? BloodPressure { get; set; }
    public int? HeartRate { get; set; }
    public int? RespiratoryRate { get; set; }
    public float? Temperature { get; set; }
    public int? OxygenSaturation { get; set; }

    public int? PainScore { get; set; }
    public string? Allergies { get; set; }

    public string? DoctorName { get; set; }

    public bool IsAdmitted { get; set; }
    public int? AdmissionId { get; set; }
}


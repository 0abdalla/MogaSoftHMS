using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models.Medical;
public class EmergencyVisit : AuditableEntity
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime ArrivalTime { get; set; } = DateTime.UtcNow;

    public EmergencySeverity Severity { get; set; }
    public EmergencyStatus Status { get; set; } = EmergencyStatus.Waiting;
    public string? ChiefComplaint { get; set; }  // الشكوى الرئيسية  => ألم صدر

    public string? BloodPressure { get; set; } // ضغط الدم
    public int? HeartRate { get; set; } // معدل ضربات القلب
    public int? RespiratoryRate { get; set; } // معدل التنفس
    public float? Temperature { get; set; } // درجة الحرارة
    public int? OxygenSaturation { get; set; } // تشبع الأكسجين
    public int? PainScore { get; set; }
    public string? Allergies { get; set; }
    public string? AssessmentNotes { get; set; } // التشخيص
    public string? TreatmentNotes { get; set; }  // العلاج

    public string? CompanionName { get; set; } = string.Empty;
    public string? CompanionPhone { get; set; } = string.Empty;
    public string? CompanionNationalId { get; set; } = string.Empty;
    public int? DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    public bool IsAdmitted { get; set; } = false;
    public int? AdmissionId { get; set; }
    public Admission? Admission { get; set; }

    public DateTime? DischargeTime { get; set; }
    public string? DischargeNotes { get; set; }
    public DateTime? LastStatusUpdate { get; set; }

    public Patient Patient { get; set; } = default!;
    public Guid EncounterNumber { get; set; } = Guid.NewGuid();
}
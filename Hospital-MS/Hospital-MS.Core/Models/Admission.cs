using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models;

public class Admission : AuditableEntity
{
    public int Id { get; set; }

    public DateTime AdmissionDate { get; set; } = DateTime.UtcNow;
    public DateTime? DischargeDate { get; set; }

    public AdmissionStatus Status { get; set; } = AdmissionStatus.Admitted;
    public AdmissionType AdmissionType { get; set; }

    public string? InitialDiagnosis { get; set; }
    public string? HealthStatus { get; set; }
    public string? Notes { get; set; }

    public bool HasCompanion { get; set; }
    public string? CompanionName { get; set; }
    public string? CompanionPhone { get; set; }
    public string? CompanionNationalId { get; set; }

    public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    public int DepartmentId { get; set; }
    public int RoomId { get; set; }
    public int BedId { get; set; }
    public PaymentMethodType? PaymentMethod { get; set; }

    public string? DischargeSummary { get; set; }

    public Guid EncounterNumber { get; set; } = Guid.NewGuid();

    public Patient Patient { get; set; } = default!;
    public Doctor Doctor { get; set; } = default!;
    public Department Department { get; set; } = default!;
    public Room Room { get; set; } = default!;
    public Bed Bed { get; set; } = default!;
    public MedicalService? MedicalService { get; set; }

    public ICollection<AdmissionCharge> Charges { get; set; } = new HashSet<AdmissionCharge>();
}

public class AdmissionCharge
{
    public int Id { get; set; }
    public int AdmissionId { get; set; }
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTime ChargeDate { get; set; } = DateTime.UtcNow;
}

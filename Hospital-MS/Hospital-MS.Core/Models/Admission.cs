namespace Hospital_MS.Core.Models
{
    public sealed class Admission : AuditableEntity
    {
        public int Id { get; set; }

        public DateTime AdmissionDate { get; set; } = DateTime.UtcNow;            


        public string? HealthStatus { get; set; }
        public string? InitialDiagnosis { get; set; }
        public string? Notes { get; set; }

        public bool HasCompanion { get; set; }
        public string? CompanionName { get; set; }
        public string? CompanionNationalId { get; set; }
        public string? CompanionPhone { get; set; }

        public int PatientId { get; set; }
        public int BedId { get; set; }
        public int DoctorId { get; set; }
        public int DepartmentId { get; set; }
        public int RoomId { get; set; }

        public Patient Patient { get; set; } = default!;
        public Bed Bed { get; set; } = default!;
        public Room Room { get; set; } = default!;
        public Doctor Doctor { get; set; } = default!;
        public Department Department { get; set; } = default!;
    }
}

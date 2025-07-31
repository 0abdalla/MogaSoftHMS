using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models
{
    public sealed class Appointment : AuditableEntity
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? ClinicId { get; set; }
        public int? MedicalServiceId { get; set; }

        public DateOnly? AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public AppointmentType Type { get; set; }
        public string? PaymentMethod { get; set; }
        public int AppointmentNumber { get; set; }

        // بيانات الطوارئ
        public string? EmergencyLevel { get; set; }
        public string? CompanionName { get; set; }
        public string? CompanionNationalId { get; set; }
        public string? CompanionPhone { get; set; }

        public Clinic? Clinic { get; set; } = default!;
        public Patient Patient { get; set; } = default!;
        public Doctor? Doctor { get; set; } = default!;
        public MedicalService? MedicalService { get; set; }
        public ICollection<MedicalServiceDetail>? MedicalServiceDetails { get; set; } = new HashSet<MedicalServiceDetail>();

        public bool IsClosed { get; set; } = false;
    }
}

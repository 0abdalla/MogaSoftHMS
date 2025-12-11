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

        public DateTime? AppointmentDate { get; set; }
        public int DurationInMinutes { get; set; } = 15;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public AppointmentType Type { get; set; }

        public PaymentMethodType? PaymentMethod { get; set; }
        public BillingStatus BillingStatus { get; set; } = BillingStatus.Unpaid;

        public int AppointmentNumber { get; set; }
        public string? CancellationReason { get; set; }

        public DateTime? CheckInTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? RoomId { get; set; }

        public Guid EncounterNumber { get; set; } = Guid.NewGuid();
        public bool IsClosed { get; set; } = false;

        // Navigation
        public Clinic? Clinic { get; set; }
        public Patient Patient { get; set; } = default!;
        public Doctor? Doctor { get; set; }
        public Room? Room { get; set; }
        public MedicalService? MedicalService { get; set; }
        public ICollection<MedicalServiceDetail> MedicalServiceDetails { get; set; } = new HashSet<MedicalServiceDetail>();
        public Invoice? Invoice { get; set; }
    }

}

using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models
{
    public class Booking : AuditableEntity
    {
        public int Id { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Currency { get; set; } = "EGP";
        public string? Notes { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public decimal Price { get; set; } = 0M;


        public int PatientId { get; set; }
        public int RoomId { get; set; }
        public int WardId { get; set; }

        public Patient Patient { get; set; } = default!;
        public Room Room { get; set; } = default!;
        public Ward Ward { get; set; } = default!;
    }
}

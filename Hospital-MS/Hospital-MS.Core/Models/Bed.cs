using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models
{
    public sealed class Bed : AuditableEntity
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public BedStatus Status { get; set; }
        public decimal DailyPrice { get; set; } = 0m;

        public int RoomId { get; set; }
        public Room Room { get; set; } = default!;
    }
}
using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models
{
    public sealed class Room : AuditableEntity
    {
        public int Id { get; set; }
        public int Number { get; set; } 
        public RoomType Type { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;
        public decimal DailyPrice { get; set; } = 0m;


        public int WardId { get; set; }
        public Ward Ward { get; set; } = default!;
        public ICollection<Bed> Beds { get; set; } = new HashSet<Bed>();
    }
}
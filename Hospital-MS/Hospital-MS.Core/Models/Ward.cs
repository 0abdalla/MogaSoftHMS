namespace Hospital_MS.Core.Models
{
    public sealed class Ward : AuditableEntity
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Capacity { get; set; }

        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
    }
}
